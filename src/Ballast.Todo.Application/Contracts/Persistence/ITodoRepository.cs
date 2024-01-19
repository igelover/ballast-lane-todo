using Ballast.Todo.Domain.Entities;

namespace Ballast.Todo.Application.Contracts.Persistence
{
    public interface ITodoRepository
    {
        Task<IEnumerable<TodoItem>> GetAllItemsAsync(string userId);
        Task<TodoItem> GetItemAsync(string id, string userId);
        Task CreateItemAsync(TodoItem item);
        Task<bool> UpdateItemAsync(string userId, TodoItem item);
        Task<bool> DeleteItemAsync(string id, string userId);
        Task<bool> MarkItemDoneAsync(string id, string userId);
    }
}
