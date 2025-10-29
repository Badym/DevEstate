namespace DevEstate.Api.Dtos;

public static class FeatureDtos
{
    public class FeatureCreateDtos
    {
        public string BuildingId { get; set; } = null!;
        public string Name { get; set; } = null!; // np. "Garaż", "Winda"
        public string? Description { get; set; }
        public decimal? Price { get; set; } // np. 40000 dla miejsca garażowego
    }

    public class FeatureUpdateDtos
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
    }

    public class FeatureResponseDtos
    {
        public string Id { get; set; } = null!;
        public string BuildingId { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public decimal? Price { get; set; }
    }
}