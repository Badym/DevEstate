using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Swashbuckle.AspNetCore.Annotations;

namespace DevEstate.Api.Models
{
    public class Company
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        // ─────────────────────────────
        // 🧩 ORYGINALNE POLA
        // ─────────────────────────────

        [BsonElement("name")]
        public string Name { get; set; } = null!;

        [BsonElement("nip")]
        public string? NIP { get; set; }

        [BsonElement("regon")]
        public string? REGON { get; set; }

        [BsonElement("krs")]
        public string? KRS { get; set; }

        [BsonElement("email")]
        public string? Email { get; set; }

        [BsonElement("phone")]
        public string? Phone { get; set; }

        [BsonElement("website")]
        public string? Website { get; set; }

        [BsonElement("address")]
        public string? Address { get; set; }

        [BsonElement("description")]
        public string? Description { get; set; }

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("logoimage")]
        public string LogoImage { get; set; }

        // ─────────────────────────────
        // 🆕 DODANE POLA (DLA RAPORTU)
        // ─────────────────────────────

        [BsonElement("legalForm")]
        public string? LegalForm { get; set; }

        [BsonElement("ceidgNumber")]
        public string? CEIDGNumber { get; set; }

        [BsonElement("fax")]
        public string? Fax { get; set; }

        [BsonElement("province")]
        public string? Province { get; set; }

        [BsonElement("county")]
        public string? County { get; set; }

        [BsonElement("municipality")]
        public string? Municipality { get; set; }

        [BsonElement("city")]
        public string? City { get; set; }

        [BsonElement("street")]
        public string? Street { get; set; }

        [BsonElement("buildingNumber")]
        public string? BuildingNumber { get; set; }

        [BsonElement("apartmentNumber")]
        public string? ApartmentNumber { get; set; }

        [BsonElement("postalCode")]
        public string? PostalCode { get; set; }
        
        [BsonElement("contactMethod")]
        public string? ContactMethod { get; set; } 
        
        
    }
}
