namespace DevEstate.Api.Dtos;

public static class PropertyDtos
{
    public class PropertyCreateDtos
    {
        public string? InvestmentId { get; set; } // null dla mieszkań
        public string? BuildingId { get; set; }   // null dla domów
        public string ApartmentNumber { get; set; } = string.Empty;
        public string Type { get; set; } = "apartment"; // apartment / house
        public double Area { get; set; }
        public double? TerraceArea { get; set; }
        public decimal Price { get; set; }
        public decimal PricePerMeter { get; set; }
        public string Status { get; set; } = "Aktualne"; // Aktualne / Zarezerwowane / Sprzedane
        
        public List<string>? RequiredFeatureIds { get; set; }

    }

    public class PropertyUpdateDtos
    {
        public decimal? Price { get; set; }
        public string? Status { get; set; }
        
        public List<string>? RequiredFeatureIds { get; set; }

    }

    public class PropertyResponseDtos
    {
        public string Id { get; set; } = string.Empty;
        public string ApartmentNumber { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public double Area { get; set; }
        public decimal Price { get; set; }
        public decimal PricePerMeter { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? BuildingId { get; set; }
        public string? InvestmentId { get; set; }
        public List<string> Images { get; set; }
        public decimal? TotalPriceWithRequiredFeatures { get; set; }
        
        public List<string>? RequiredFeatureIds { get; set; }
    }
}