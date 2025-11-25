namespace DevEstate.Dtos.DeveloperOpenData;

// ====================================================================
// DTO DO LISTY DATASETÓW — używane TYLKO przez DatasetFinder
// ====================================================================

// Odpowiedź API z /search
public class ApiDatasetListResponse
{
    public List<ApiDatasetRawItem>? Data { get; set; }
}

// Pojedynczy wpis z listy
public class ApiDatasetRawItem
{
    public string? Id { get; set; }
    public string? Type { get; set; }
    public ApiDatasetAttributes? Attributes { get; set; }
}

// Atrybuty datasetu
public class ApiDatasetAttributes
{
    public string? Model { get; set; }
    public string? Title { get; set; }
}

// DTO wysyłane do Frontu
public class ApiDatasetItem
{
    public string? Id { get; set; }
    public string? Title { get; set; }
    public string? Model { get; set; }
}