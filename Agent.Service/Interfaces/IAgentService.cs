using Agent.Models;

namespace Agent.Service.Interfaces;

public interface IAgentService
{
    Task<AgentStatistics> GetCurrentShiftTeamTotalAssignmentsAndCapacity(bool includeOverflowTeam = true);
    Task<Dal.Data.Entities.Agent?> GetNextAvailableAgentAsync();
    Task<Dal.Data.Entities.Agent?> GetNextAvailableOverflowTeamAgentAsync();
    Task<Dal.Data.Entities.Agent?> GetAgentAsync(int id);
    Task CreateAgentAsync(AgentModel agentModel);
    Task<bool> UpdateAgentAsync(int id, AgentModel agentModel);
    Task<bool> DeleteAgentAsync(int id);
    Task<bool> UpdateAgentStatusAsync(int id, bool isActive);
}

