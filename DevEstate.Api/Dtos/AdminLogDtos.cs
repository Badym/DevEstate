namespace DevEstate.Api.Dtos;

public static class AdminLogDtos
{
    public class AdminLogResponseDtos
    {
        public string Id { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string Action { get; set; } = null!;
        public string Entity { get; set; } = null!;
        public string EntityId { get; set; } = null!;
        public DateTime Timestamp { get; set; }
    }

    public class AdminLogFilterDtos
    {
        public string? UserName { get; set; }
        public string? Action { get; set; }     // CREATE / UPDATE / DELETE
        public string? Entity { get; set; }     // Building / Feature / Property etc.
    }
}