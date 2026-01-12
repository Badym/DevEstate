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


            string appDirectory = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles");
            Directory.CreateDirectory(appDirectory); 

            // Pobieramy nazwę pliku z URL
            string fileName = Path.GetFileName(url);
            string extension = Path.GetExtension(fileName).ToLower();

            // Jeśli plik nie jest CSV ani XLSX, zwróć null
            if (extension != ".csv" && extension != ".xlsx")
            {
                return null;
            }

            // Generowanie unikalnej nazwy pliku
            string uniqueFileName = Guid.NewGuid().ToString("N") + extension;
            string filePath = Path.Combine(appDirectory, uniqueFileName);

            // Zapisz plik w docelowej lokalizacji
            await File.WriteAllBytesAsync(filePath, bytes);

            // Logowanie ścieżki pliku
            Console.WriteLine($"Downloaded file saved to: {filePath}");

            // Zwróć ścieżkę do lokalnego pliku
            return filePath;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error downloading file: {ex.Message}");
            return null;
        }
    }

}
