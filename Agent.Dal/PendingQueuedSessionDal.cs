using Agent.Dal.Data.Entities;
using Agent.Dal.Data.Interfaces;
using Agent.Dal.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Agent.Dal
{
    public class PendingQueuedSessionDal(IAgentDbContext agentDbContext) : IPendingQueuedSessionDal
    {
        public async Task Create(PendingQueuedSession pendingQueuedSession)
        {
            await agentDbContext.PendingQueuedSessions.AddAsync(pendingQueuedSession);
            await agentDbContext.SaveChangesAsync();
        }

        public async Task<PendingQueuedSession?> GetFirst()
        {
            return await agentDbContext.PendingQueuedSessions.OrderBy(p => p.Id).FirstOrDefaultAsync();
        }

        public async Task<int> GetTotalPendingSessions()
        {
            return await agentDbContext.PendingQueuedSessions.CountAsync();
        }

        public async Task<bool> Delete(Guid sessionId)
        {
            var pendingQueuedSession =
                await agentDbContext.PendingQueuedSessions.FirstOrDefaultAsync(p => p.SessionId == sessionId);

            if (pendingQueuedSession == null)
                return false;

            agentDbContext.PendingQueuedSessions.Remove(pendingQueuedSession);
            await agentDbContext.SaveChangesAsync();
            return true;
        }
    }
}
