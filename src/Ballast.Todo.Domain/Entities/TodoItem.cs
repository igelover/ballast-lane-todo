using MongoDB.Bson.Serialization.Attributes;
using static Ballast.Todo.Domain.Enums;

namespace Ballast.Todo.Domain.Entities
{
    public class TodoItem
    {
        [BsonId]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string Description { get; set; } = string.Empty;

        public TodoStatus Status { get; set; } = TodoStatus.Pending;

        public string UserId { get; set; } = string.Empty;
    }
}
