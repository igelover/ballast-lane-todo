using Ballast.Todo.Application.Contracts.Persistence;
using Ballast.Todo.Domain.Entities;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using static Ballast.Todo.Domain.Enums;

namespace Ballast.Todo.Infrastructure.Database.Repositories
{
    public class TodoRepository : ITodoRepository
    {
        private readonly IDataContext _context;
        private readonly ILogger<TodoRepository> _logger;

        public TodoRepository(IDataContext context, ILogger<TodoRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task CreateItemAsync(TodoItem item)
        {
            _logger.LogDebug("Creating item {item}", item.Id);
            await _context.TodoItems.InsertOneAsync(item);
        }

        public async Task<TodoItem> GetItemAsync(string id, string userId)
        {
            _logger.LogDebug("Getting item {item}", id);
            var items = await _context.TodoItems.FindAsync(x => x.Id.Equals(id) && x.UserId.Equals(userId));
            return await items.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<TodoItem>> GetAllItemsAsync(string userId)
        {
            _logger.LogDebug("Getting items for user {user}", userId);
            var items = await _context.TodoItems.FindAsync(x => x.UserId.Equals(userId));
            return await items.ToListAsync();
        }

        public async Task<bool> UpdateItemAsync(string userId, TodoItem item)
        {
            _logger.LogDebug("Updatind item {item}", item.Id);
            var updateResult = await _context.TodoItems.ReplaceOneAsync(filter: g => g.Id.Equals(item.Id) && g.UserId.Equals(userId), replacement: item);
            return updateResult != null && updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
        }

        public async Task<bool> DeleteItemAsync(string id, string userId)
        {
            _logger.LogDebug("Deleting item {item}", id);
            var filter = Builders<TodoItem>.Filter.Eq(x => x.Id, id);
            var deleteResult = await _context.TodoItems.DeleteOneAsync(filter);
            return deleteResult != null && deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
        }

        public async Task<bool> MarkItemDoneAsync(string id, string userId)
        {
            _logger.LogDebug("Marking item as done {item}", id);
            var filter = Builders<TodoItem>.Filter.Eq(x => x.Id, id);
            var update = Builders<TodoItem>.Update.Set("Status", TodoStatus.Done);
            var updateResult = await _context.TodoItems.UpdateOneAsync(filter, update);
            return updateResult != null && updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
        }
    }
}
