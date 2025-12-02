using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DevEstate.Models
{
    public class DeveloperPriceEntity
    {
        [BsonId] // Automatyczne generowanie ID przez MongoDB
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }
        public string? Wojewodztwo { get; set; }
        public string? Powiat { get; set; }
        public string? Gmina { get; set; }
        public string? Miejscowosc { get; set; }
        public decimal CenaZaM2Total { get; set; }

        // Liczba mieszkań
        public int LiczbaMieszkan { get; set; }

        // Obliczona średnia cena za m2 na podstawie sumy i liczby mieszkań
        public decimal CenaZaM2AVG => LiczbaMieszkan > 0 ? Math.Round(CenaZaM2Total / LiczbaMieszkan, 2) : 0;

        public DateTime LastUpdated { get; set; }  // Data ostatniej aktualizacji
    }
}