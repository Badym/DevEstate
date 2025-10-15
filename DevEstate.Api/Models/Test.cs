using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DevEstate.Api.Models
{
    public class Test
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = null!;

        [BsonElement("name")]
        public string Name { get; set; } = null!;

        [BsonElement("description")]
        public string Description { get; set; } = null!;
    }
}