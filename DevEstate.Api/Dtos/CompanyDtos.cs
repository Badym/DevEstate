namespace DevEstate.Api.Dtos;

public static class CompanyDtos
{
    public class Create
    {
        public string Name { get; set; } = null!;
        public string NIP { get; set; }
        public string REGON { get; set; }
        public string KRS { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Website { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
    }

    public class Update
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Website { get; set; }
        public string? Address { get; set; }
        public string? Description { get; set; }
    }

    public class Response
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Website { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}