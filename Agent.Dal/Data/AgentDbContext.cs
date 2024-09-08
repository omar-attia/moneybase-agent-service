using Agent.Dal.Data.Entities;
using Agent.Dal.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Agent.Dal.Data;

public class AgentDbContext(DbContextOptions<AgentDbContext> options) : DbContext(options), IAgentDbContext
{
    public DbSet<Team> Teams { get; set; }
    public DbSet<Entities.Agent> Agents { get; set; }
    public DbSet<AgentAssignment> AgentAssignments { get; set; }
    public DbSet<SeniorityMultiplier> SeniorityMultipliers { get; set; }
    public DbSet<PendingQueuedSession> PendingQueuedSessions { get; set; }

    public async Task<int> SaveChangesAsync()
    {
        return await base.SaveChangesAsync();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Team>()
            .HasIndex(team => team.ShiftStartTime)
            .HasDatabaseName("IX_Team_ShiftStartTime");

        modelBuilder.Entity<Team>()
            .HasIndex(team => team.ShiftEndTime)
            .HasDatabaseName("IX_Team_ShiftEndTime");

        modelBuilder.Entity<Team>()
            .HasIndex(team => team.IsOfficeHours)
            .HasDatabaseName("IX_Team_IsOfficeHours");

        modelBuilder.Entity<Entities.Agent>()
            .HasIndex(agent => agent.IsActive)
            .HasDatabaseName("IX_Agent_IsActive");

        modelBuilder.Entity<Entities.Agent>()
            .HasIndex(agent => agent.IsOverflow)
            .HasDatabaseName("IX_Agent_IsOverflow");

        modelBuilder.Entity<Entities.Agent>()
            .HasIndex(agent => agent.IsDeleted)
            .HasDatabaseName("IX_Agent_IsDeleted");

        modelBuilder.Entity<Entities.Agent>()
            .HasIndex(agent => agent.MaxConcurrentChats)
            .HasDatabaseName("IX_Agent_MaxConcurrentChats");

        modelBuilder.Entity<PendingQueuedSession>()
            .HasIndex(agent => agent.SessionId)
            .HasDatabaseName("IX_PendingQueuedSession_SessionId")
            .IsUnique();

        modelBuilder.Entity<AgentAssignment>()
            .HasIndex(agent => agent.SessionId)
            .HasDatabaseName("IX_AgentAssignment_SessionId")
            .IsUnique();

        modelBuilder.Entity<AgentAssignment>()
            .HasIndex(agent => agent.IsCompleted)
            .HasDatabaseName("IX_AgentAssignment_IsCompleted");

        base.OnModelCreating(modelBuilder);
    }
}
