using Agent.Dal.Data.Entities;
using Agent.Dal.Interfaces;
using Agent.Service.Interfaces;

namespace Agent.Service
{
    public class PendingQueuedSessionService(IPendingQueuedSessionDal pendingQueuedSessionDal) : IPendingQueuedSessionService
    {
        public async Task Create(PendingQueuedSession pendingQueuedSession)
        {
            await pendingQueuedSessionDal.Create(pendingQueuedSession);
        }

        public async Task<bool> Delete(Guid sessionId)
        {
            return await pendingQueuedSessionDal.Delete(sessionId);
        }

        public async Task<PendingQueuedSession?> GetFirst()
        {
            return await pendingQueuedSessionDal.GetFirst();
        }

        public async Task<int> GetTotalPendingSessions()
        {
            return await pendingQueuedSessionDal.GetTotalPendingSessions();
        }
    }
}
