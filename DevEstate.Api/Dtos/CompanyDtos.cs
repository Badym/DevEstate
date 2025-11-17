namespace DevEstate.Api.Dtos;

public static class CompanyDtos
{
    public class CompanyCreateDtos
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

    public class CompanyUpdateDtos
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Website { get; set; }
        public string? Address { get; set; }
        public string? Description { get; set; }
    }

    public class CompanyResponseDtos
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Website { get; set; }
        public string Address { get; set; }
        public string REGON { get; set; }
        public string Description { get; set; }
        public string KRS { get; set; }
        public string NIP { get; set; }
        public DateTime CreatedAt { get; set; }
        public string LogoImage { get; set; }
    }
    
    public class CompanyDto
    {
        public string Name { get; set; }
        public string Website { get; set; }
    }
    
}
