using Agent.Dal.Data.Entities;
using Agent.Dal.Data.Interfaces;
using Agent.Dal.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Agent.Dal;

public class TeamDal(IAgentDbContext agentDbContext) : ITeamDal
{
    public async Task CreateTeamAsync(Team team)
    {
        await agentDbContext.Teams.AddAsync(team);
        await agentDbContext.SaveChangesAsync();
    }

    public async Task<Team?> GetTeamAsync(int id)
    {
        return await agentDbContext.Teams.AsNoTracking().FirstOrDefaultAsync(team => team.Id == id);
    }

    public async Task<bool> UpdateTeamAsync(int id, Team team)
    {
        var existingTeam = await agentDbContext.Teams.FirstOrDefaultAsync(team => team.Id == id);
        
        if (existingTeam == null)
            return false;

        existingTeam.Name = team.Name;
        existingTeam.ShiftStartTime = team.ShiftStartTime;
        existingTeam.ShiftEndTime = team.ShiftEndTime;
        existingTeam.IsOfficeHours = team.IsOfficeHours;
        existingTeam.UpdatedAt = DateTime.UtcNow;

        await agentDbContext.SaveChangesAsync();

        return true;
    }

    public async Task<bool?> IsOfficeHours()
    {
        var currentTime = DateTime.UtcNow.TimeOfDay;

        return (await agentDbContext.Teams.AsNoTracking().FirstOrDefaultAsync(team =>
            (team.ShiftStartTime <= currentTime && team.ShiftEndTime >= currentTime) ||
            (team.ShiftStartTime > team.ShiftEndTime &&
             (currentTime >= team.ShiftStartTime || currentTime <= team.ShiftEndTime))))?.IsOfficeHours;
    }
}
