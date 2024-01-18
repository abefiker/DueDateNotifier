using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DueDateNotifier.Models
{
    public class TaskModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("Name")]
        public string? TaskName { get; set; }

        [BsonElement("Description")]
        public string? Description { get; set; }

        [BsonElement("DueDate")]
        public DateTime? DueDate { get; set; }

        [BsonElement("IsCompleted")]
        public bool? IsCompleted { get; set; }

        [BsonElement("AssignedUser")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? AssignedUser { get; set; }
    }
}
