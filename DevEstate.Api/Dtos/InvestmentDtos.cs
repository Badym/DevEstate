namespace DevEstate.Api.Dtos;

public static class InvestmentDtos
{
    public class InvestmentCreateDtos
    {
        public string CompanyId { get; set; } = "69009df36e02ddca1d2c18b4";
        public string Name { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Street { get; set; } = null!;
        public string PostalCode { get; set; } = null!;
        public string Description { get; set; }
        public string Status { get; set; } = "Aktualne"; // Na sprzedaż / Zarezerwowane / Nieaktualne
    }

    public class InvestmentUpdateDtos
    {
        public string? Name { get; set; }
        public string? City { get; set; }
        public string? Street { get; set; }
        public string? PostalCode { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }
    }

    public class InvestmentResponseDtos
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Street { get; set; } = null!;
        public string PostalCode { get; set; } = null!;
        public string? Description { get; set; }
        public string Status { get; set; } = "Aktualne";
        public DateTime CreatedAt { get; set; }
        public List<string> Images { get; set; } 
    }
}