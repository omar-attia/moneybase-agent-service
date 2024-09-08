using Agent.Dal.Data.Entities;

namespace Agent.Dal.Interfaces
{
    public interface IAgentAssignmentDal
    {
        Task<AgentAssignment> GetAssignmentAsync(int id);
        Task CreateAssignmentAsync(AgentAssignment assignment);
        Task<bool> UpdateAssignmentAsync(int id, AgentAssignment assignment);
        Task<bool> MarkSessionCompleted(Guid sessionId);

    }
}
