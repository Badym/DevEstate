namespace DevEstate.Api.Dtos
{
    public static class FeatureTypeDtos
    {
        public class FeatureTypeCreateDtos
        {
            public string Name { get; set; } = null!;
            public string? UnitName { get; set; }
            public bool IsActive { get; set; } = true;
        }

        public class FeatureTypeUpdateDtos
        {
            public string? Name { get; set; }
            public string? UnitName { get; set; }
            public bool? IsActive { get; set; }
        }

        public class FeatureTypeResponseDtos
        {
            public string? Id { get; set; }
            public string Name { get; set; } = null!;
            public string? UnitName { get; set; }
            public bool IsActive { get; set; }
        }
    }
}