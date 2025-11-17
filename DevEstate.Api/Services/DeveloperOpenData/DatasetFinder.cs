using System.Text.Json;
using DevEstate.Dtos.DeveloperOpenData;

namespace DevEstate.Services.DeveloperOpenData;

public class DatasetFinder
{
    private readonly HttpClient _client;

    public DatasetFinder(HttpClient client)
    {
        _client = client;
    }

    public async Task<List<ApiDatasetItem>> GetDeveloperDatasetsAsync()
    {
        var all = new List<ApiDatasetItem>();
        int page = 1;

        string[] keywords =
        {
            "deweloper",
            "dewelopera",
            "ceny ofertowe",
            "mieszkań",
            "mieszkanie",
            "lokal mieszkalny"
        };

        while (true)
        {
            var url = $"https://api.dane.gov.pl/1.4/search?page={page}&per_page=100&model[terms]=dataset&lang=pl";

            Console.WriteLine("FETCH: " + url);

            var json = await _client.GetStringAsync(url);

            var parsed = JsonSerializer.Deserialize<ApiDatasetListResponse>(
                json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );

            if (parsed?.Data == null || parsed.Data.Count == 0)
                break;

            foreach (var item in parsed.Data)
            {
                var title = item.Attributes?.Title?.ToLower() ?? "";

                if (keywords.Any(k => title.Contains(k)))
                {
                    all.Add(new ApiDatasetItem
                    {
                        Id = item.Id,
                        Title = item.Attributes?.Title,
                        Model = item.Attributes?.Model
                    });
                }
            }

            if (parsed.Data.Count < 100)
                break;

            page++;
        }

        return all;
    }
}