namespace DevEstate.Api.Dtos;

public static class BuildingDtos
{
    public class Create
    {
        public string InvestmentId { get; set; } = null!;
        public string BuildingNumber { get; set; } = null!;
        public string Description { get; set; }
        public string Status { get; set; } = "Aktualne"; // Na sprzedaż / Zarezerwowane / Nieaktualne
    }

    public class Update
    {
        public string? BuildingNumber { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }
    }

    public class Response
    {
        public string Id { get; set; } = null!;
        public string BuildingNumber { get; set; } = null!;
        public string Description { get; set; }
        public string Status { get; set; } = "Aktualne";
        public DateTime CreatedAt { get; set; }
    }
}