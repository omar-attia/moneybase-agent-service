using Agent.Dal.Data.Entities;

namespace Agent.Service.Interfaces
{
    public interface IPendingQueuedSessionService
    {
        Task Create(PendingQueuedSession pendingQueuedSession);
        Task<PendingQueuedSession?> GetFirst();
        Task<int> GetTotalPendingSessions();
        Task<bool> Delete(Guid sessionId);
    }
}
