using Agent.Dal.Data.Interfaces;
using Agent.Dal.Interfaces;
using Agent.Models;
using Microsoft.EntityFrameworkCore;

namespace Agent.Dal;

public class AgentDal(IAgentDbContext agentDbContext) : IAgentDal
{
    public async Task<AgentStatistics> GetCurrentShiftTeamTotalAssignmentsAndCapacity(bool includeOverflowTeam = true)
    {
        var capacity = await GetCurrentShiftAgentsQuery(includeOverflowTeam).SumAsync(agent => agent.MaxConcurrentChats);
        var assignments = await GetCurrentShiftAgentsQuery(includeOverflowTeam).SelectMany(agent => agent.Assignments)
            .CountAsync();

        return new AgentStatistics
        {
            Assignments = assignments,
            Capacity = capacity
        };
    }

    public async Task<Data.Entities.Agent?> GetNextAvailableOverflowTeamAgentAsync()
    {
        return await GetCurrentShiftAgentsQuery()
            .OrderBy(agent => agent.Assignments.Count)
            .FirstOrDefaultAsync();
    }

    public async Task<Data.Entities.Agent?> GetNextAvailableAgentAsync()
    {
        return await GetCurrentShiftAgentsQuery(false)
            .OrderBy(agent => agent.SeniorityId)
            .ThenBy(agent => agent.Assignments.Count)
            .FirstOrDefaultAsync();
    }

    public async Task CreateAgentAsync(Data.Entities.Agent agent)
    {
        await agentDbContext.Agents.AddAsync(agent);
        await agentDbContext.SaveChangesAsync();
    }

    public async Task<bool> DeleteAgentAsync(int id)
    {
        var agent = await agentDbContext.Agents.FirstOrDefaultAsync(agent => agent.Id == id);
        if (agent == null)
            return false;

        agent.IsDeleted = true;
        await agentDbContext.SaveChangesAsync();
        return true;
    }

    public async Task<Data.Entities.Agent?> GetAgentAsync(int id)
    {
        return await agentDbContext.Agents.AsNoTracking().FirstOrDefaultAsync(agent => agent.Id == id);
    }

    public async Task<bool> UpdateAgentAsync(int id, Data.Entities.Agent agent)
    {
        var existingAgent = await agentDbContext.Agents.FirstOrDefaultAsync(a => a.Id == id);
        
        if (existingAgent == null)
            return false;

        existingAgent.Name = string.IsNullOrWhiteSpace(agent.Name) ? existingAgent.Name : agent.Name;
        existingAgent.Email = string.IsNullOrWhiteSpace(agent.Email) ? existingAgent.Email : agent.Email;
        existingAgent.SeniorityId = agent.SeniorityId > 0 ? agent.SeniorityId : existingAgent.SeniorityId;
        existingAgent.TeamId = agent.TeamId > 0 ? agent.TeamId : existingAgent.TeamId;
        existingAgent.MaxConcurrentChats = agent.MaxConcurrentChats > 0 ? agent.MaxConcurrentChats : existingAgent.MaxConcurrentChats;
        existingAgent.IsActive = agent.IsActive;
        existingAgent.IsOverflow = agent.IsOverflow;

        await agentDbContext.SaveChangesAsync();

        return true;
    }

    public async Task<bool> UpdateAgentStatusAsync(int id, bool isActive)
    {
        var agent = await agentDbContext.Agents.FirstOrDefaultAsync(agent => agent.Id == id);
        if (agent == null)
            return false;

        agent.IsActive = isActive;
        await agentDbContext.SaveChangesAsync();
        return true;
    }

    private IQueryable<Data.Entities.Agent> GetCurrentShiftAgentsQuery(bool includeOverflowTeam = true)
    {
        var currentTime = DateTime.UtcNow.TimeOfDay;

        return agentDbContext.Agents
            .Include(agent => agent.SeniorityMultiplier)
            .Include(agent => agent.Assignments) // Explicitly include assignments
            .Include(agent => agent.Team)
            .Where(agent =>
                // Count only incomplete assignments and ensure agent has fewer than max concurrent chats
                agent.Assignments.Count(a => !a.IsCompleted) < agent.MaxConcurrentChats &&
                !agent.IsDeleted &&
                agent.IsActive &&
                agent.IsOverflow == includeOverflowTeam &&
                (
                    // Check if the current time is within the agent's shift
                    (agent.Team.ShiftStartTime <= currentTime && agent.Team.ShiftEndTime >= currentTime) ||
                    (agent.Team.ShiftStartTime > agent.Team.ShiftEndTime && (currentTime >= agent.Team.ShiftStartTime ||
                                                                             currentTime <= agent.Team.ShiftEndTime))
                ));
    }
}
