using Ballast.Todo.Domain.Entities;
using MongoDB.Driver;

namespace Ballast.Todo.Infrastructure.Database
{
    public interface IDataContext
    {
        IMongoCollection<User> Users { get; }
        IMongoCollection<Session> Sessions { get; }
        IMongoCollection<TodoItem> TodoItems { get; }
    }
}
