using Agent.Dal.Data.Entities;
using Agent.Models;

namespace Agent.Service.Interfaces
{
    public interface IAgentAssignmentService
    {
        Task ListenForSessions(string message);
        Task<AgentAssignment> GetAssignmentAsync(int id);
        Task AssignChatSessionToAgent(Guid sessionId);
        Task<bool> UpdateAssignmentAsync(int id, AgentAssignmentModel agentAssignmentModel);
        Task<bool> MarkSessionCompleted(AgentAssignmentModel agentAssignmentModel);
    }
}
