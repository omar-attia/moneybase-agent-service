using Agent.Dal.Data.Entities;
using Agent.Dal.Interfaces;
using Agent.Models;
using Agent.Service.Interfaces;
using Mapster;

namespace Agent.Service;

public class TeamService(ITeamDal teamDal) : ITeamService
{
    public async Task CreateTeam(TeamModel teamModel)
    {
        var team = teamModel.Adapt<Team>();
        await teamDal.CreateTeamAsync(team);
    }

    public async Task<Team?> GetTeamAsync(int id)
    {
        return await teamDal.GetTeamAsync(id);
    }

    public async Task<bool> IsOfficeHours()
    {
        return await teamDal.IsOfficeHours() ?? false;
    }

    public async Task<bool> UpdateTeamAsync(int id, TeamModel teamModel)
    {
        var team = teamModel.Adapt<Team>();
        return await teamDal.UpdateTeamAsync(id, team);
    }
}

