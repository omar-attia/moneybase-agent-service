using Agent.Dal.Data.Entities;
using Agent.Models;

namespace Agent.Service.Interfaces;
public interface ITeamService
{
    Task CreateTeam(TeamModel teamModel);
    Task<Team?> GetTeamAsync(int id);
    Task<bool> UpdateTeamAsync(int id, TeamModel teamModel);
    Task<bool> IsOfficeHours();
}

