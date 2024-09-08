namespace Agent.Service.Interfaces;

public interface IOverflowTeamService
{
    Task<Dal.Data.Entities.Agent> GetOverflowAgentAsync(int id);
}

