namespace DevEstate.Api.Dtos
{
    public static class FeatureDtos
    {
        public class FeatureCreateDtos
        {
            public string InvestmentId { get; set; } = null!;
            public string? BuildingId { get; set; }
            public string FeatureTypeId { get; set; } = null!;
            public decimal? Price { get; set; }
            public bool IsAvailable { get; set; } = true;
            public string? Description { get; set; }
            public bool IsRequired { get; set; }
        }

        public class FeatureUpdateDtos
        {
            public string? InvestmentId { get; set; }
            public string? BuildingId { get; set; }
            public string? FeatureTypeId { get; set; }
            public decimal? Price { get; set; }
            public bool? IsAvailable { get; set; }
            public bool? IsRequired { get; set; }
            public string? Description { get; set; }
        }

        public class FeatureResponseDtos
        {
            public string? Id { get; set; }
            public string InvestmentId { get; set; } = null!;
            public string? BuildingId { get; set; }
            public string FeatureTypeId { get; set; } = null!;
            public decimal? Price { get; set; }
            public bool IsAvailable { get; set; }
            public bool IsRequired { get; set; }
            public string? Description { get; set; }
        }
        
        public class FeatureTypeSummaryDto
        {
            public string FeatureTypeId { get; set; } = null!;
            public string FeatureTypeName { get; set; } = null!;
            public decimal? MinPrice { get; set; }  // dla wyświetlania np. "od 25 000 zł"
            public decimal? MaxPrice { get; set; }
            public int Count { get; set; }          // liczba wystąpień tego typu
        }

    }
}