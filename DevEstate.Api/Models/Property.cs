using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Swashbuckle.AspNetCore.Annotations;

namespace DevEstate.Api.Models
{
    public class Property
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("investmentId")]
        public string InvestmentId { get; set; } = null!;

        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("buildingId")]
        public string? BuildingId { get; set; } // null dla domów jednorodzinnych

        [BsonElement("apartmentNumber")]
        public string ApartmentNumber { get; set; } = null!; // np. "A2" lub "33E"

        [BsonElement("type")]
        public string Type { get; set; } = "apartment"; // apartment / house    

        [BsonElement("area")]
        public double Area { get; set; }

        [BsonElement("terraceArea")]
        public double? TerraceArea { get; set; }

        [BsonElement("price")]
        public decimal Price { get; set; }
        
        [BsonElement("totalPriceWithRequiredFeatures")]
        public decimal? TotalPriceWithRequiredFeatures { get; set; }

        [BsonElement("pricePerMeter")]
        public decimal PricePerMeter { get; set; }

        [BsonElement("status")]
        public string Status { get; set; } = "Aktualne"; // Aktualne / Zarezerwowane / Sprzedane
        
        [BsonElement("images")]
        public List<string> Images { get; set; } = new();
    }
}