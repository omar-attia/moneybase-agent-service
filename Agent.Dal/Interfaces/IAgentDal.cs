using Agent.Models;

namespace Agent.Dal.Interfaces
{
    public interface IAgentDal
    {
        Task<AgentStatistics> GetCurrentShiftTeamTotalAssignmentsAndCapacity(bool includeOverflowTeam = true);
        Task<Data.Entities.Agent?> GetNextAvailableAgentAsync();
        Task<Data.Entities.Agent?> GetNextAvailableOverflowTeamAgentAsync();
        Task<Data.Entities.Agent?> GetAgentAsync(int id);
        Task CreateAgentAsync(Data.Entities.Agent? agent);
        Task<bool> UpdateAgentAsync(int id, Data.Entities.Agent agent);
        Task<bool> DeleteAgentAsync(int id);
        Task<bool> UpdateAgentStatusAsync(int id, bool isActive);
    }
}
