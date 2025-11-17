namespace DevEstate.Api.Dtos
{
    public static class CompanyDetailsDtos
    {
        public class ResponseCompanyDetails
        {
            public string? Id { get; set; }
            public string Name { get; set; } = null!;
            public string? LegalForm { get; set; }
            public string? KRS { get; set; }
            public string? CEIDGNumber { get; set; }
            public string? NIP { get; set; }
            public string? REGON { get; set; }
            public string? Phone { get; set; }
            public string? Fax { get; set; }
            public string? Email { get; set; }
            public string? Website { get; set; }

            public string? Province { get; set; }
            public string? County { get; set; }
            public string? Municipality { get; set; }
            public string? City { get; set; }
            public string? Street { get; set; }
            public string? BuildingNumber { get; set; }
            public string? ApartmentNumber { get; set; }
            public string? PostalCode { get; set; }

            public string? Description { get; set; }
            public string? LogoImage { get; set; }
            
            public string ContactMethond { get; set; }
        }

        public class UpdateCompanyDetails
        {
            public string Name { get; set; } = null!;
            public string? LegalForm { get; set; }
            public string? KRS { get; set; }
            public string? CEIDGNumber { get; set; }
            public string? NIP { get; set; }
            public string? REGON { get; set; }
            public string? Phone { get; set; }
            public string? Fax { get; set; }
            public string? Email { get; set; }
            public string? Website { get; set; }

            public string? Province { get; set; }
            public string? County { get; set; }
            public string? Municipality { get; set; }
            public string? City { get; set; }
            public string? Street { get; set; }
            public string? BuildingNumber { get; set; }
            public string? ApartmentNumber { get; set; }
            public string? PostalCode { get; set; }

            public string? Description { get; set; }
            public string? LogoImage { get; set; }
            
            public string ContactMethond { get; set; }        }
    }
}
