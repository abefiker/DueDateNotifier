using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace DueDateNotifier.Models
{
    public class Users
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        [BsonElement("UserName")]
        public string? Name { get; set; }
        [BsonElement("Email")]
        public string? Email { get; set; };
        [BsonElement("Password")]
        public string? Password { get; set; }
    }
}
