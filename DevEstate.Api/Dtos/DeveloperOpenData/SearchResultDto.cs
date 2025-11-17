namespace DevEstate.Dtos.DeveloperOpenData;

public class SearchResultDto
{
    public List<SearchItemDto>? data { get; set; }
}

public class SearchItemDto
{
    public string id { get; set; }
    public SearchAttributesDto attributes { get; set; }
}

public class SearchAttributesDto
{
    public string title { get; set; }
    public string description { get; set; }
    public string publisher { get; set; }
}