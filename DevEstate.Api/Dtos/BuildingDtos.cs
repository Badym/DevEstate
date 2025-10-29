namespace DevEstate.Api.Dtos;

public static class BuildingDtos
{
    public class BuildingCreateDtos
    {
        public string InvestmentId { get; set; } = null!;
        public string BuildingNumber { get; set; } = null!;
        public string Description { get; set; }
        public string Status { get; set; } = "Aktualne"; // Na sprzedaż / Zarezerwowane / Nieaktualne
    }

    public class BuildingUpdateDtos
    {
        public string? BuildingNumber { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }
    }

    public class BuildingResponseDtos
    {
        public string Id { get; set; } = null!;
        public string InvestmentId { get; set; } = null!;
        public string BuildingNumber { get; set; } = null!;
        public string Description { get; set; }
        public string Status { get; set; } = "Aktualne";
        public List<string> Images { get; set; } = new();
        public DateTime CreatedAt { get; set; }
        
        public int AvailablePropertiesCount { get; set; }
        public int SoldPropertiesCount { get; set; }
    }
}