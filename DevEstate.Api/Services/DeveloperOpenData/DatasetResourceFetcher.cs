using System.Text.Json;
using DevEstate.Dtos.DeveloperOpenData;

namespace DevEstate.Services.DeveloperOpenData
{
    public class DatasetResourceFetcher
    {
        private readonly HttpClient _client;

        public DatasetResourceFetcher(HttpClient client)
        {
            _client = client;
        }

        /// <summary>
        /// Zwraca URL do najnowszego dostępnego pliku CSV (lub XLSX jeśli CSV brak).
        /// </summary>
        public async Task<string?> GetLatestFileUrlAsync(string datasetId)
        {
            var url = $"https://api.dane.gov.pl/1.4/datasets/{datasetId}/resources";

            var json = await _client.GetStringAsync(url);

            var parsed = JsonSerializer.Deserialize<ApiResourceListResponse>(
                json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );

            if (parsed?.Data == null || parsed.Data.Count == 0)
                return null;

            // wybieramy te, które mają datę
            var validResources = parsed.Data
                .Where(r => r.Attributes != null && !string.IsNullOrWhiteSpace(r.Attributes.Data_Date))
                .ToList();

            if (validResources.Count == 0)
                return null;

            // sortujemy od najnowszego
            var latest = validResources
                .OrderByDescending(r => DateTime.Parse(r.Attributes!.Data_Date!))
                .First();

            // preferuj CSV
            if (!string.IsNullOrWhiteSpace(latest.Attributes!.Csv_File_Url))
                return latest.Attributes.Csv_File_Url;

            // fallback: XLSX
            if (!string.IsNullOrWhiteSpace(latest.Attributes.File_Url))
                return latest.Attributes.File_Url;

            // ostateczność: csv_download_url
            return latest.Attributes.Csv_Download_Url;
        }
    }
}