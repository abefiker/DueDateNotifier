using AspNetCore.Identity.MongoDbCore.Models;
using MongoDbGenericRepository.Attributes;

namespace DueDateNotifier.Models
{
    [CollectionName("todos")]
    public class ApplicationTodos : MongoIdentityUser<Guid>
    {
        public string? TaskName { get; internal set; }
        public string? Description { get; internal set; }
        public DateTime DueDate { get; internal set; }
        public string? UserId { get; internal set; }
    }
}
