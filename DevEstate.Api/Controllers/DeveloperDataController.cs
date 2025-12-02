using DevEstate.Services.DeveloperOpenData;
using Microsoft.AspNetCore.Mvc;
using System.IO;

[ApiController]
[Route("api/developer-data")]
public class DeveloperDataController : ControllerBase
{
    private readonly DatasetFinder _finder;
    private readonly DatasetResourceFetcher _details;
    private readonly CsvDataParser _csvDataParser;
    private readonly CsvDownloader _downloader;
    private readonly DeveloperOpenDataParser _DataParserCSVXLSX;
    private readonly AveragePriceService _averagePriceService;
    
    public DeveloperDataController(
        DatasetFinder finder,
        DatasetResourceFetcher details,
        CsvDataParser csvDataParser,
        CsvDownloader downloader,
        DeveloperOpenDataParser DataParser,
        AveragePriceService averagePriceService)
    {
        _details = details;
        _finder = finder;
        _csvDataParser = csvDataParser;
        _downloader = downloader;
        _DataParserCSVXLSX = DataParser;
        _averagePriceService = averagePriceService;
    }

    [HttpGet("all-datasets")]
    public async Task<IActionResult> AllDatasets()
    {
        var data = await _finder.GetDeveloperDatasetsAsync();
        return Ok(data);
    }

    [HttpGet("latest-file/{datasetId}")]
    public async Task<IActionResult> GetLatestFile(string datasetId)
    {
        // Pobieramy najnowszy plik (CSV lub XLSX)
        var fileUrl = await _details.GetLatestFileUrlAsync(datasetId);
    
        if (string.IsNullOrWhiteSpace(fileUrl))
        {
            return NotFound(new { message = "No valid file found for the given dataset" });
        }
    
        // Zwracamy link do pliku
        return Ok(new { fileUrl });
    }
    
    [HttpGet("sorted-data")]
    public async Task<IActionResult> GetSortedData(string datasetId)
    {
        // Sprawdzamy czy datasetId jest poprawne
        if (string.IsNullOrEmpty(datasetId))
        {
            return BadRequest(new { message = "Dataset ID is required" });
        }

        // Pobieramy URL pliku CSV
        var fileUrl = await _details.GetLatestFileUrlAsync(datasetId);

        if (string.IsNullOrWhiteSpace(fileUrl))
        {
            return NotFound(new { message = "No valid file found for the given dataset" });
        }

        // Logowanie URL pliku
        Console.WriteLine($"File URL: {fileUrl}");

        // Pobieramy plik do lokalnej ścieżki
        var localPath = await _downloader.DownloadToTempFileAsync(fileUrl);
        if (localPath == null)
        {
            return StatusCode(500, new { message = "Unable to download the file" });
        }

        // Logowanie ścieżki do pliku w konsoli
        Console.WriteLine($"Downloaded file saved to: {localPath}");

        try
        {
            // Parsowanie CSV
            var records = _DataParserCSVXLSX.Parse(localPath);

            // Sortowanie danych po województwie, powiecie, gminie, miejscowości oraz cenie za m²
            var sortedRecords = records
                .OrderBy(r => r.Wojewodztwo)
                .ThenBy(r => r.Powiat)
                .ThenBy(r => r.Gmina)
                .ThenBy(r => r.Miejscowosc)
                .ThenBy(r => r.CenaZaM2)
                .ToList();

            return Ok(sortedRecords);
        }
        catch (Exception ex)
        {
            // Logowanie błędu parsowania CSV
            Console.WriteLine($"Error parsing the CSV file: {ex.Message}");
            return StatusCode(500, new { message = "Error parsing the CSV file", details = ex.Message });
        }
        finally
        {
            // Usuwanie pliku tymczasowego po przetworzeniu
            try
            {
                // Jeżeli plik istnieje, usuń go
                if (System.IO.File.Exists(localPath))
                {
                    System.IO.File.Delete(localPath);
                    Console.WriteLine($"Temporary file {localPath} deleted successfully.");
                }
            }
            catch (Exception ex)
            {
                // Logowanie problemu przy usuwaniu pliku
                Console.WriteLine($"Error deleting temp file: {ex.Message}");
            }

        }
    }
    
    [HttpGet("average-price")]
    public IActionResult GetAveragePrice()
    {
        // Uruchamiamy proces w tle (nie czekamy na zakończenie)
        _ = Task.Factory.StartNew(
            async () =>
            {
                try
                {
                    await _averagePriceService.ProcessAllDatasetsAsync();
                }
                catch (Exception ex)
                {
                    // Tu użyj loggera zamiast Console.WriteLine
                    Console.WriteLine($"[AveragePrice] Background error: {ex}");
                }
            },
            CancellationToken.None,
            TaskCreationOptions.DenyChildAttach,
            TaskScheduler.Default
        ); 

        // Zgodnie z REST 202 Accepted = przyjęto zadanie do wykonania
        return Accepted(new 
        { 
            message = "Batch processing started in background.",
            startedAt = DateTime.UtcNow
        });
    }


}



