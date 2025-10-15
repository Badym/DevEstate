namespace DevEstate.Api.Dtos;

public static class PropertyDtos
{
    public class Create
    {
        public string InvestmentId { get; set; } = null!;
        public string? BuildingId { get; set; } // null dla domów
        public string ApartmentNumber { get; set; } = null!;
        public string Type { get; set; } = "apartment"; // apartment / house
        public double Area { get; set; }
        public double? TerraceArea { get; set; }
        public decimal Price { get; set; }
        public int PricePerMeter { get; set; }
        public string Status { get; set; } = "Aktualne"; // Aktualne / Zarezerwowane / Sprzedane
    }

    public class Update
    {
        public decimal? Price { get; set; }
        public string? Status { get; set; }
    }

    public class Response
    {
        public string Id { get; set; } = null!;
        public string ApartmentNumber { get; set; } = null!;
        public string Type { get; set; } = null!;
        public double Area { get; set; }
        public decimal Price { get; set; }
        public int PricePerMeter { get; set; }
        public string Status { get; set; } = null!;
        public string? BuildingNumber { get; set; }
        public string InvestmentName { get; set; } = null!;
    }
}