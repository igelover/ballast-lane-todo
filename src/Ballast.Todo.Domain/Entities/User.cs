using MongoDB.Bson.Serialization.Attributes;

namespace Ballast.Todo.Domain.Entities
{
    public class User
    {
        [BsonId]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
    }
}
