
using Microsoft.EntityFrameworkCore;
using Agent.Dal.Data.Entities;

namespace Agent.Dal.Data.Interfaces;

public interface IAgentDbContext
{
    DbSet<Team> Teams { get; set; }
    DbSet<Entities.Agent> Agents { get; set; }
    DbSet<AgentAssignment> AgentAssignments { get; set; }
    DbSet<SeniorityMultiplier> SeniorityMultipliers { get; set; }
    DbSet<PendingQueuedSession> PendingQueuedSessions { get; set; }
    Task<int> SaveChangesAsync();
}
