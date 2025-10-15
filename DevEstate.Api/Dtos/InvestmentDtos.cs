namespace DevEstate.Api.Dtos;

public static class InvestmentDtos
{
    public class Create
    {
        public string CompanyId { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Street { get; set; } = null!;
        public string PostalCode { get; set; } = null!;
        public string Description { get; set; }
        public string Status { get; set; } = "Aktualne"; // Na sprzedaż / Zarezerwowane / Nieaktualne
    }

    public class Update
    {
        public string? Name { get; set; }
        public string? City { get; set; }
        public string? Street { get; set; }
        public string? PostalCode { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }
    }

    public class Response
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Street { get; set; } = null!;
        public string PostalCode { get; set; } = null!;
        public string Description { get; set; }
        public string Status { get; set; } = "Aktualne";
        public DateTime CreatedAt { get; set; }
    }
}