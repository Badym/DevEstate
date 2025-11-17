namespace DevEstate.Api.Dtos
{
    public static class ProspectReportDtos
    {
        public class Row
        {
            // 🏢 Deweloper
            public string? DeveloperName { get; set; }
            public string? LegalForm { get; set; }
            public string? KRS { get; set; }
            public string? CEIDGNumber { get; set; }
            public string? NIP { get; set; }
            public string? REGON { get; set; }
            public string? Phone { get; set; }
            public string? Fax { get; set; }
            public string? Email { get; set; }
            public string? Website { get; set; }

            public string? HeadquartersProvince { get; set; }
            public string? HeadquartersCounty { get; set; }
            public string? HeadquartersMunicipality { get; set; }
            public string? HeadquartersCity { get; set; }
            public string? HeadquartersStreet { get; set; }
            public string? HeadquartersBuildingNumber { get; set; }
            public string? HeadquartersApartmentNumber { get; set; }
            public string? HeadquartersPostalCode { get; set; }

            // 🏗️ Inwestycja / lokalizacja
            public string? InvestmentName { get; set; }
            public string? InvestmentProvince { get; set; }
            public string? InvestmentCounty { get; set; }
            public string? InvestmentMunicipality { get; set; }
            public string? InvestmentCity { get; set; }
            public string? InvestmentStreet { get; set; }
            public string? InvestmentBuildingNumber { get; set; }
            public string? InvestmentPostalCode { get; set; }
            public string? ContactMethod { get; set; }

            // 🏠 Nieruchomość
            public string? PropertyType { get; set; }
            public string? ApartmentNumber { get; set; }
            public double? Area { get; set; }
            public decimal? PricePerM2 { get; set; }
            public decimal? TotalPrice { get; set; }
            public decimal? FullPrice { get; set; }
            public DateTime? PriceFromDate { get; set; }

            // 🚗 Dodatki (feature’y przypisane)
            public string? AttachedParts { get; set; }       // np. "Miejsce postojowe"
            public string? AttachedPartsLabels { get; set; } // np. "MP-14, MP-15"
            public decimal? AttachedPartsPrice { get; set; }
            public DateTime? AttachedPartsPriceFromDate { get; set; }

            // 🌐 Link do prospektu
            public string? ProspectUrl { get; set; }
        }
    }
}
