using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Swashbuckle.AspNetCore.Annotations;

namespace DevEstate.Api.Models
{
    public class Document
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("propertyId")]
        public string? PropertyId { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("buildingId")]
        public string? BuildingId { get; set; }

        [BsonElement("fileName")]
        public string FileName { get; set; } = null!;

        [BsonElement("fileType")]
        public string FileType { get; set; } = null!; // np. image/jpeg, application/pdf

        [BsonElement("fileUrl")]
        public string FileUrl { get; set; } = null!; // link do pliku (np. w chmurze)

        [BsonElement("uploadedAt")]
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
    }
}