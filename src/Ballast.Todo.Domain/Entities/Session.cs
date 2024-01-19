using MongoDB.Bson.Serialization.Attributes;

namespace Ballast.Todo.Domain.Entities
{
    public class Session
    {
        [BsonId]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string UserId { get; set; }
        public DateTimeOffset CreateDate { get; set; } = DateTimeOffset.Now;
        public string TokenId { get; set; } = string.Empty;
        public bool Active { get; set; } = true;
    }
}
