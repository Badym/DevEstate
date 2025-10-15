namespace DevEstate.Api.Dtos;

public static class DocumentDtos
{
    public class Create
    {
        public string PropertyId { get; set; } = null!;
        public string BuildingId { get; set; } = null!;
        public string FileName { get; set; } = null!;
        public string FileType { get; set; } = null!;
        public string FileUrl { get; set; } = null!;
    }

    public class Response
    {
        public string Id { get; set; } = null!;
        public string PropertyId { get; set; } = null!;
        public string BuildingId { get; set; } = null!;
        public string FileName { get; set; } = null!;
        public string FileType { get; set; } = null!;
        public string FileUrl { get; set; } = null!;
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
    }
}