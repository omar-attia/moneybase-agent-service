using Agent.Dal.Data.Entities;

namespace Agent.Dal.Interfaces
{
    public interface IPendingQueuedSessionDal
    {
        Task Create(PendingQueuedSession pendingQueuedSession);
        Task<PendingQueuedSession?> GetFirst();

        Task<int> GetTotalPendingSessions();
        Task<bool> Delete(Guid sessionId);
    }
}
