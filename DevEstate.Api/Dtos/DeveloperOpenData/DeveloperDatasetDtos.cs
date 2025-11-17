namespace DevEstate.Dtos.DeveloperOpenData;

// -------------------------------
// GŁÓWNA ODPOWIEDŹ API
// -------------------------------
public class ApiDatasetListResponse
{
    public List<ApiDatasetRawItem>? Data { get; set; }
}

// -------------------------------
// POJEDYNCZY RAW ITEM
// -------------------------------
public class ApiDatasetRawItem
{
    public string? Id { get; set; }
    public string? Type { get; set; }
    public ApiDatasetAttributes? Attributes { get; set; }
}

// -------------------------------
// ATRYBUTY ITEMU (title, model)
// -------------------------------
public class ApiDatasetAttributes
{
    public string? Model { get; set; }
    public string? Title { get; set; }
}

// -------------------------------
// DTO, KTÓRE ZWRACAMY DO FE
// -------------------------------
public class ApiDatasetItem
{
    public string? Id { get; set; }
    public string? Title { get; set; }
    public string? Model { get; set; }
}