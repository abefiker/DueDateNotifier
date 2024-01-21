using AspNetCore.Identity.MongoDbCore.Models;
using MongoDB.Driver;
using MongoDbGenericRepository.Attributes;

namespace DueDateNotifier.Models
{
    [CollectionName("roles")]
    public class ApplicationRole:MongoIdentityRole<Guid>
    {

    }
}
