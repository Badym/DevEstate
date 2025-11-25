using System.Net;

namespace DevEstate.Services.DeveloperOpenData;

public class CsvDownloader
{
    private readonly HttpClient _http;

    public CsvDownloader(HttpClient http)
    {
        _http = http;
    }

    public async Task<string?> DownloadToTempFileAsync(string url)
    {
        try
        {
            var bytes = await _http.GetByteArrayAsync(url);

            // katalog temp
            string tempDir = Path.Combine(Path.GetTempPath(), "devestate_csv");
            Directory.CreateDirectory(tempDir);

            // nazwa pliku
            string fileName = Guid.NewGuid().ToString("N") + ".csv";
            string filePath = Path.Combine(tempDir, fileName);

            await File.WriteAllBytesAsync(filePath, bytes);

            return filePath;
        }
        catch
        {
            return null;
        }
    }
}