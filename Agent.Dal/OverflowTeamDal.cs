using Agent.Dal.Data.Interfaces;
using Agent.Dal.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Agent.Dal;

public class OverflowTeamDal(IAgentDbContext agentDbContext) : IOverflowTeamDal
{
    public async Task CreateOverflowAgentAsync(Data.Entities.Agent overflowAgent)
    {
        await agentDbContext.Agents.AddAsync(overflowAgent);
        await agentDbContext.SaveChangesAsync();
    }

    public async Task<Data.Entities.Agent> GetOverflowAgentAsync(int id)
    {
        return await agentDbContext.Agents.FirstOrDefaultAsync(agent =>
            agent.Id == id && agent.IsOverflow && !agent.IsDeleted);
    }
}

