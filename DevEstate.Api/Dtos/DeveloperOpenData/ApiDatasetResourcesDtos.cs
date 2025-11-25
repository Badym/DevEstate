using System.Collections.Generic;

namespace DevEstate.Dtos.DeveloperOpenData
{
    // ============================
    // Root response
    // ============================
    public class ApiResourceListResponse
    {
        public List<ApiResourceItem>? Data { get; set; }
    }

    // ============================
    // Resource item
    // ============================
    public class ApiResourceItem
    {
        public string? Id { get; set; }
        public ApiResourceAttributes? Attributes { get; set; }
    }

    // ============================
    // Resource attributes
    // ============================
    public class ApiResourceAttributes
    {
        public string? Title { get; set; }

        // "2025-09-29"
        public string? Data_Date { get; set; }

        // pełny link do CSV
        public string? Csv_File_Url { get; set; }

        // alternatywny link (czasem nieużyteczny)
        public string? Csv_Download_Url { get; set; }

        // fallback — zwykle XLSX
        public string? File_Url { get; set; }
    }
}