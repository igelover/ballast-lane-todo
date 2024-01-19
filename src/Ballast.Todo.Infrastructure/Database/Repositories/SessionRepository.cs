using Ballast.Todo.Application.Contracts.Persistence;
using Ballast.Todo.Domain.Entities;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Ballast.Todo.Infrastructure.Database.Repositories
{
    public class SessionRepository : ISessionRepository
    {
        private readonly IDataContext _context;
        private readonly ILogger<SessionRepository> _logger;

        public SessionRepository(IDataContext context, ILogger<SessionRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<bool> DeleteSessionsAsync(string userId)
        {
            _logger.LogDebug("Deleting sessions for user {user}", userId);
            var filter = Builders<Session>.Filter.Eq(x => x.UserId, userId);
            var deleteResult = await _context.Sessions.DeleteManyAsync(filter);
            return deleteResult != null && deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
        }

        public async Task CreateSessionAsync(Session session)
        {
            await _context.Sessions.InsertOneAsync(session);
            _logger.LogDebug("Created new session {session} for user {user}", session.Id, session.UserId);
        }
    }
}
