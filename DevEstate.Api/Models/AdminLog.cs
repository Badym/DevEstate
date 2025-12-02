using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DevEstate.Api.Models;

public class AdminLog
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string UserName  { get; set; } = null!;

    public string Action { get; set; } = null!;  // ADD / UPDATE / DELETE
    public string Entity { get; set; } = null!;  // User, FeatureType, etc.
    public string EntityId { get; set; } = null!;

    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}