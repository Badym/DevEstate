namespace DevEstate.Api.Dtos;

public static class DocumentDtos
{
    public class DocumentCreateDtos
    {
        public string? InvestmentId { get; set; }
        public string? BuildingId { get; set; }
        public string? PropertyId { get; set; }

        public string FileName { get; set; } = null!;
        public string FileType { get; set; } = null!;
        public string FileUrl { get; set; } = null!;
        public string? Type { get; set; } // np. "Prospekt", "Rzut", "Załącznik"
    }

    public class DocumentResponseDtos
    {
        public string Id { get; set; } = null!;
        public string? InvestmentId { get; set; }
        public string? BuildingId { get; set; }
        public string? PropertyId { get; set; }

        public string FileName { get; set; } = null!;
        public string FileType { get; set; } = null!;
        public string FileUrl { get; set; } = null!;
        public string? Type { get; set; }

        public DateTime UploadedAt { get; set; }
    }
}
