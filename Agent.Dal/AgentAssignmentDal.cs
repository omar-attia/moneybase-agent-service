using Agent.Dal.Data.Entities;
using Agent.Dal.Data.Interfaces;
using Agent.Dal.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Agent.Dal;

public class AgentAssignmentDal(IAgentDbContext agentDbContext) : IAgentAssignmentDal
{
    public async Task CreateAssignmentAsync(AgentAssignment assignment)
    {
        await agentDbContext.AgentAssignments.AddAsync(assignment);
        await agentDbContext.SaveChangesAsync();
    }

    public async Task<AgentAssignment> GetAssignmentAsync(int id)
    {
        return await agentDbContext.AgentAssignments.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<bool> MarkSessionCompleted(Guid sessionId)
    {
        var agentAssignment = await agentDbContext.AgentAssignments.FirstOrDefaultAsync(a => a.SessionId == sessionId);
        if (agentAssignment == null)
            return false;

        agentAssignment.IsCompleted = true;
        await agentDbContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateAssignmentAsync(int id, AgentAssignment assignment)
    {
        var agentAssignment = await agentDbContext.AgentAssignments.FirstOrDefaultAsync(a => a.Id == id);
        if (agentAssignment == null)
            return false;

        agentAssignment.AgentId = assignment.AgentId;
        agentAssignment.IsCompleted = assignment.IsCompleted;
        agentAssignment.SessionId = assignment.SessionId;

        await agentDbContext.SaveChangesAsync();

        return true;
    }
}

