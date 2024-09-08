using Agent.Dal.Interfaces;
using Agent.Models;
using Agent.Models.Enums;
using Agent.Service.Interfaces;
using Mapster;
using Microsoft.Extensions.Configuration;

namespace Agent.Service
{
    public class AgentService(IAgentDal agentDal, IConfiguration configuration) : IAgentService
    {
        public async Task CreateAgentAsync(AgentModel agentModel)
        {
            TypeAdapterConfig<AgentModel, Dal.Data.Entities.Agent>.NewConfig()
                .Map(dest => dest.SeniorityId, src => src.Seniority);
            var agent = agentModel.Adapt<Dal.Data.Entities.Agent>();
            var agentMaxConcurrentChats = Convert.ToInt32(configuration["EnvironmentVariables:AgentMaxConcurrentChats"]);
            var seniorityLevel = (SeniorityLevel)agent.SeniorityId;
            agent.MaxConcurrentChats = Convert.ToInt32(CalculateAgentMaxCapacity(seniorityLevel, agentMaxConcurrentChats));
            await agentDal.CreateAgentAsync(agent);
        }

        public async Task<bool> DeleteAgentAsync(int id)
        {
            return await agentDal.DeleteAgentAsync(id);
        }

        public async Task<Dal.Data.Entities.Agent?> GetAgentAsync(int id)
        {
            var agent = await agentDal.GetAgentAsync(id);
            if (agent == null)
                throw new KeyNotFoundException("Agent not found");

            return agent;
        }

        public async Task<Dal.Data.Entities.Agent?> GetNextAvailableAgentAsync()
        {
            return await agentDal.GetNextAvailableAgentAsync();
        }

        public async Task<bool> UpdateAgentAsync(int id, AgentModel agentModel)
        {
            var agent = agentModel.Adapt<Dal.Data.Entities.Agent>();
            return await agentDal.UpdateAgentAsync(id, agent);
        }

        public async Task<bool> UpdateAgentStatusAsync(int id, bool isActive)
        {
            return await agentDal.UpdateAgentStatusAsync(id, isActive);
        }

        private static decimal CalculateAgentMaxCapacity(SeniorityLevel seniorityLevel, int agentMaxConcurrentChats)
        {
            return seniorityLevel switch
            {
                SeniorityLevel.Junior => SeniorityMultipliers.Multipliers[SeniorityLevel.Junior] * agentMaxConcurrentChats,
                SeniorityLevel.MidLevel => SeniorityMultipliers.Multipliers[SeniorityLevel.MidLevel] * agentMaxConcurrentChats,
                SeniorityLevel.Senior => SeniorityMultipliers.Multipliers[SeniorityLevel.Senior] * agentMaxConcurrentChats,
                SeniorityLevel.TeamLead => SeniorityMultipliers.Multipliers[SeniorityLevel.TeamLead] * agentMaxConcurrentChats,
                _ => 0
            };
        }

        public async Task<AgentStatistics> GetCurrentShiftTeamTotalAssignmentsAndCapacity(bool includeOverflowTeam = true)
        {
            return await agentDal.GetCurrentShiftTeamTotalAssignmentsAndCapacity(includeOverflowTeam);
        }

        public async Task<Dal.Data.Entities.Agent?> GetNextAvailableOverflowTeamAgentAsync()
        {
            return await agentDal.GetNextAvailableOverflowTeamAgentAsync();
        }
    }
}
