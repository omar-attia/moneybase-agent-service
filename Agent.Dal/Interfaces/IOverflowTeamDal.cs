namespace Agent.Dal.Interfaces;

public interface IOverflowTeamDal
{
    Task<Data.Entities.Agent> GetOverflowAgentAsync(int id);
    Task CreateOverflowAgentAsync(Data.Entities.Agent overflowAgent);
}

