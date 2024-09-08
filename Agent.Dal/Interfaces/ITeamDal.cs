using Agent.Dal.Data.Entities;

namespace Agent.Dal.Interfaces;

public interface ITeamDal
{
    Task CreateTeamAsync(Team team);
    Task<Team?> GetTeamAsync(int id);
    Task<bool> UpdateTeamAsync(int id, Team team);
    Task<bool?> IsOfficeHours();
}

