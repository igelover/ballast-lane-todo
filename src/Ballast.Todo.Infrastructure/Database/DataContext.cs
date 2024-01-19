using System.Diagnostics.CodeAnalysis;
using Ballast.Todo.Domain.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Ballast.Todo.Infrastructure.Database
{
    [ExcludeFromCodeCoverage]
    public class DataContext : IDataContext
    {
        public DataContext()
        {
        }

        public DataContext(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            var database = client.GetDatabase(configuration.GetValue<string>("DatabaseSettings:DatabaseName"));

            Users = database.GetCollection<User>(configuration.GetValue<string>("DatabaseSettings:UserCollection"));
            Sessions = database.GetCollection<Session>(configuration.GetValue<string>("DatabaseSettings:SessionCollection"));
            TodoItems = database.GetCollection<TodoItem>(configuration.GetValue<string>("DatabaseSettings:TodoCollection"));
        }

        public virtual IMongoCollection<User> Users { get; }
        public virtual IMongoCollection<Session> Sessions { get; }
        public virtual IMongoCollection<TodoItem> TodoItems { get; }
    }
}