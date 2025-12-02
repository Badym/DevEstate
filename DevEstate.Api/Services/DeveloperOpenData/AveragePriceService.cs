using DevEstate.Api.Services;
using DevEstate.Dtos.DeveloperOpenData;
using DevEstate.Services.DeveloperOpenData;

namespace DevEstate.Services.DeveloperOpenData;

public class AveragePriceService
{
    private readonly DatasetFinder _finder;
    private readonly DatasetResourceFetcher _details;
    private readonly DeveloperOpenDataParser _parser;
    private readonly CsvDownloader _downloader;
    private readonly DeveloperPriceService _priceService;
    private readonly ILogger<AveragePriceService> _logger;

    public AveragePriceService(
        DatasetFinder finder,
        DatasetResourceFetcher details,
        DeveloperOpenDataParser parser,
        CsvDownloader downloader,
        DeveloperPriceService priceService,
        ILogger<AveragePriceService> logger)
    {
        _finder = finder;
        _details = details;
        _parser = parser;
        _downloader = downloader;
        _priceService = priceService;
        _logger = logger;
    }

    public async Task ProcessAllDatasetsAsync()
    {
        _logger.LogInformation("Starting batch update of developer prices...");

        var datasets = await _finder.GetDeveloperDatasetsAsync();
        int datasetIndex = 0;

        foreach (var dataset in datasets)
        {
            datasetIndex++;
            _logger.LogInformation($"[{datasetIndex}/{datasets.Count}] Processing dataset {dataset.Id}...");

            try
            {
                var fileUrl = await _details.GetLatestFileUrlAsync(dataset.Id);
                if (string.IsNullOrWhiteSpace(fileUrl))
                {
                    _logger.LogWarning($"Dataset {dataset.Id} has no downloadable file.");
                    continue;
                }

                var localPath = await _downloader.DownloadToTempFileAsync(fileUrl);

                if (localPath == null)
                {
                    _logger.LogError($"Failed to download file for dataset {dataset.Id}");
                    continue;
                }

                _logger.LogInformation($"Downloaded dataset {dataset.Id} → {localPath}");

                // CSV/XLSX parser → lista rekordów
                var records = _parser.Parse(localPath);

                foreach (var r in records)
                {
                    // pomijamy niekompletne albo bez ceny
                    if (r.Wojewodztwo == null || r.Powiat == null || r.CenaZaM2 == null)
                        continue;

                    var updateDto = new DeveloperPriceRecordUpdateDto
                    {
                        Wojewodztwo = r.Wojewodztwo,
                        Powiat = r.Powiat,
                        CenaZaM2 = r.CenaZaM2
                    };

                    await _priceService.AddOrUpdateAsync(updateDto);

                    _logger.LogInformation(
                        $"Updated/Added record: {r.Wojewodztwo}/{r.Powiat} (+1 mieszkanie, +{r.CenaZaM2} zł/m2)");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error processing dataset {dataset.Id}: {ex.Message}");
            }
        }

        _logger.LogInformation("Batch update completed!");
    }
}
