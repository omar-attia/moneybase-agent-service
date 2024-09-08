using Agent.Dal.Data.Entities;
using Agent.Dal.Interfaces;
using Agent.Models;
using Agent.Models.Enums;
using Agent.Service.Interfaces;
using Mapster;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Polly;
using RabbitMQLibrary.Interfaces;
using System.Transactions;
using Microsoft.Extensions.Logging;

namespace Agent.Service
{
    public class AgentAssignmentService(IAgentAssignmentDal agentAssignmentDal, IAgentService agentService, IPendingQueuedSessionService pendingQueuedSessionService, ITeamService teamService, 
        IConfiguration configuration, ILogger<AgentAssignmentService> logger, IRabbitMQService rabbitMqService) : IAgentAssignmentService
    {
        private const decimal MaxQueueMultiplier = 1.5m;
        private const int DefaultRetryCount = 3;
        private const int DefaultTimeInSecondsBetweenAttempts = 5;

        private async Task HandleNewSession(SessionModel session)
        {
            var retryCountConfig = configuration["EnvironmentVariables:RetryCount"];
            var secondsBetweenRetryAttemptsConfig = configuration["EnvironmentVariables:secondsBetweenRetryAttempts"];
            // Policy for retrying the agent assignment
            var retryPolicy = Policy
                .Handle<Exception>() // Handle failures (such as no agents available)
                .WaitAndRetryAsync(
                    retryCount: string.IsNullOrWhiteSpace(retryCountConfig)
                        ? DefaultRetryCount
                        : Convert.ToInt32(retryCountConfig),
                    sleepDurationProvider: retryAttempt =>
                        TimeSpan.FromSeconds(string.IsNullOrWhiteSpace(secondsBetweenRetryAttemptsConfig)
                            ? DefaultTimeInSecondsBetweenAttempts
                            : Convert.ToInt32(secondsBetweenRetryAttemptsConfig)), // Wait seconds between attempts
                    onRetry: (exception, timeSpan, retryCount, context) =>
                    {
                        if (retryCount >= DefaultRetryCount) //mark session status inactive if trying to pull more than 3 times
                        {
                            PublishSessionStatus(session.Id, SessionStatus.Inactive);
                        }

                        Console.WriteLine(
                            $"Retry {retryCount} for session: {session.Id}. Exception: {exception.Message}");
                    });

            await retryPolicy.ExecuteAsync(async () => await AssignChatSessionToAgent(session.Id));
        }

        public async Task AssignChatSessionToAgent(Guid sessionId)
        {
            var statistics = await agentService.GetCurrentShiftTeamTotalAssignmentsAndCapacity(includeOverflowTeam: false);
            var maxQueueLength = GetMaxQueueLength(statistics.Capacity);
            var totalPendingSessions = await pendingQueuedSessionService.GetTotalPendingSessions();

            if (IsCapacityAvailable(statistics))
            {
                var agent = await agentService.GetNextAvailableAgentAsync();
                await AssignAgentToSessionAsync(agent, sessionId);
                return;
            }

            if (CanQueueSession(maxQueueLength, totalPendingSessions, statistics.Capacity))
            {
                await QueueSession(sessionId);
            }
            else
            {
                await HandleOverflowOrReject(sessionId);
            }
        }


        public async Task<AgentAssignment> GetAssignmentAsync(int id)
        {
            return await agentAssignmentDal.GetAssignmentAsync(id);
        }

        public async Task<bool> UpdateAssignmentAsync(int id, AgentAssignmentModel agentAssignmentModel)
        {
            var agentAssignment = agentAssignmentModel.Adapt<AgentAssignment>();
            var result = await agentAssignmentDal.UpdateAssignmentAsync(id, agentAssignment);
            if(result && agentAssignment.IsCompleted)
                PublishSessionStatus(agentAssignment.SessionId, SessionStatus.Closed);
            return result;
        }

        public async Task<bool> MarkSessionCompleted(AgentAssignmentModel agentAssignmentModel)
        {
            using var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                // Mark the session as completed
                if (!await agentAssignmentDal.MarkSessionCompleted(agentAssignmentModel.SessionId))
                {
                    return false;
                }
                
                PublishSessionStatus(agentAssignmentModel.SessionId, SessionStatus.Closed);

                // Commit the transaction even if no pending session exists
                var pendingQueuedSession = await pendingQueuedSessionService.GetFirst();
                if (pendingQueuedSession != null)
                {
                    await agentAssignmentDal.CreateAssignmentAsync(new AgentAssignment()
                    {
                        AgentId = agentAssignmentModel.AgentId,
                        SessionId = pendingQueuedSession.SessionId
                    });

                    await pendingQueuedSessionService.Delete(pendingQueuedSession.SessionId);
                }

                transactionScope.Complete();
                return true;
            }
            catch (Exception ex)
            {
                // Log the exception
                return false;
            }
        }

        private void PublishSessionStatus(Guid sessionId, SessionStatus status)
        {
            var sessionStatusString =
                JsonConvert.SerializeObject(new SessionQueueModel(sessionId, status));
            rabbitMqService.PublishMessage(configuration["RabbitMq:Queues:PublishQueue"],sessionStatusString);
        }

        public async Task ListenForSessions(string message)
        {
            if (message != null)
            {
                var session = JsonConvert.DeserializeObject<SessionModel>(message);
                await HandleNewSession(session);
            }
        }
       
        private async Task AssignAgentToSessionAsync(Dal.Data.Entities.Agent agent, Guid sessionId)
        {
            if (agent == null)
                throw new InvalidOperationException("Queue is full. Chat request refused.");

            var agentAssignment = new AgentAssignment()
            {
                AgentId = agent.Id,
                SessionId = sessionId,
            };

            await agentAssignmentDal.CreateAssignmentAsync(agentAssignment);
            PublishSessionStatus(sessionId, SessionStatus.Assigned);
        }

        private async Task HandleOverflowOrReject(Guid sessionId)
        {
            if (!await teamService.IsOfficeHours())
                throw new InvalidOperationException("Queue is full. Chat request refused.");

            var agent = await agentService.GetNextAvailableOverflowTeamAgentAsync();
            await AssignAgentToSessionAsync(agent, sessionId);
        }
        private async Task QueueSession(Guid sessionId)
        {
            await pendingQueuedSessionService.Create(new PendingQueuedSession
            {
                SessionId = sessionId
            });
        }

        private int GetMaxQueueLength(int capacity)
        {
            var maxQueueLengthFactorConfig = configuration["EnvironmentVariables:MaxQueueLengthFactor"];
            return (int)Math.Floor(capacity * (string.IsNullOrWhiteSpace(maxQueueLengthFactorConfig) ? MaxQueueMultiplier : Convert.ToDecimal(maxQueueLengthFactorConfig)));
        }

        private static bool IsCapacityAvailable(AgentStatistics statistics)
        {
            return statistics.Assignments < statistics.Capacity;
        }

        private static bool CanQueueSession(int maxQueueLength, int totalPendingSessions, int capacity)
        {
            return maxQueueLength > totalPendingSessions + capacity;
        }

        public async Task HandleMessage(string message)
        {
            if (message != null)
            {
                var session = JsonConvert.DeserializeObject<SessionModel>(message);
                await HandleNewSession(session);
            }
        }
    }
}
