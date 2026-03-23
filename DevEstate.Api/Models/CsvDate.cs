using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DevEstate.Api.Models
{
    public class CsvDate
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("date")]
        public DateTime Date { get; set; }

        [BsonElement("fileName")]
        public string FileName { get; set; } = null!;
    }
}