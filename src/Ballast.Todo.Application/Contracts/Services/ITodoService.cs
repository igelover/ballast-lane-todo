using Ballast.Todo.Domain.DTO;
using Ballast.Todo.Domain.Entities;

namespace Ballast.Todo.Application.Contracts.Services
{
    public interface ITodoService
    {
        Task<IEnumerable<TodoItem>> GetAllItemsAsync(string userId);
        Task<TodoItem> GetItemAsync(string id, string userId);
        Task<TodoItem> CreateItemAsync(string userId, TodoItemDTO item);
        Task<TodoItem> UpdateItemAsync(string id, string userId, TodoItem item);
        Task DeleteItemAsync(string id, string userId);
        Task MarkItemDoneAsync(string id, string userId);
    }
}
