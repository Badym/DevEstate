using DevEstate.Api.Dtos;

namespace DevEstate.Api.Services;

public class XmlFeedGenerationService
{
    private readonly ProspectReportService _reportService;
    private readonly Md5Service _md5Service;
    private readonly XmlPriceFeedService _xmlService;
    private readonly IWebHostEnvironment _env;
    private readonly CompanyDtos.CompanyDto _company;
    private readonly CsvDateService _csvDateService;
    private readonly GitHubService _gitHubService;

    public XmlFeedGenerationService(
        ProspectReportService reportService,
        Md5Service md5Service,
        XmlPriceFeedService xmlService,
        IWebHostEnvironment env,
        CompanyDtos.CompanyDto company,
        CsvDateService csvDateService,
        GitHubService gitHubService)
    {
        _reportService = reportService;
        _md5Service = md5Service;
        _xmlService = xmlService;
        _env = env;
        _company = company;
        _csvDateService = csvDateService;
        _gitHubService = gitHubService;
    }

    //public async Task<(string csvUrl, string xmlUrl, string md5Url, string xmlPath)> GenerateAsync(string folderName = "dane")
    //{
    //    await _reportService.GenerateCsvReportAsync(folderName);

    //    var csvUrl = $"{_company.Website}/{folderName}/cennik.csv";

    //    var resource = new XmlResourceInfoDto
    //    {
    //        CsvUrl = csvUrl,
    //        DataDate = DateTime.Today
    //    };

    //    var outputDir = Path.Combine(_env.WebRootPath, folderName);
    //    var xmlPath = await _xmlService.GenerateXml_3(resource, outputDir);

    //    _md5Service.SaveMd5File(xmlPath);

    //    var xmlUrl = $"{_company.Website}/{folderName}/cennik.xml";
    //    var md5Url = $"{_company.Website}/{folderName}/cennik.md5";
    
        
    //    return (csvUrl, xmlUrl, md5Url, xmlPath);
    //}
    
    public async Task<(string csvUrl, string xmlUrl, string md5Url, string xmlPath)> GenerateAsync(string folderName = "dane")
    {
        var polandTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
        var nowPl = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, polandTimeZone);


        var today = DateTime.SpecifyKind(nowPl.Date, DateTimeKind.Utc);
        Console.WriteLine("UTC: " + DateTime.UtcNow);
        Console.WriteLine("PL: " + today);
        // 1️⃣ jeśli już jest wpis na dziś → nie rób nic
        var existing = await _csvDateService.GetByDateAsync(today);
        if (existing != null)
        {
            var existingUrl = $"https://raw.githubusercontent.com/Badym/DevEstate/main/ceny";

            var resourceExisting = new XmlResourceInfoDto
            {
                CsvUrl = existingUrl,
                DataDate = today
            };

            var outputDir = Path.Combine(_env.WebRootPath, folderName);
            var xmlPathExisting = await _xmlService.GenerateXml_3(resourceExisting, outputDir);

            _md5Service.SaveMd5File(xmlPathExisting);

            return (
                existingUrl,
                $"{_company.Website}/{folderName}/cennik.xml",
                $"{_company.Website}/{folderName}/cennik.md5",
                xmlPathExisting
            );
        }

        // 2️⃣ generuj CSV tymczasowy
        await _reportService.GenerateCsvReportAsync(folderName);

        var tempCsvPath = Path.Combine(_env.WebRootPath, folderName, "cennik.csv");
        
        // test
        var firstFilePath = Path.Combine(_env.WebRootPath, folderName, "cennik_1.csv");

        if (!File.Exists(firstFilePath))
        {
            // kopiuj jako pierwszy plik
           // File.Copy(tempCsvPath, firstFilePath, true);

            // 🚀 upload na GitHub
            var repoPath = "ceny/cennik_1.csv";
            await _gitHubService.UploadFileAsync(tempCsvPath, repoPath);

            Console.WriteLine("🔥 Uploaded FIRST file: cennik_1.csv to GitHub");
        }
        ////////////////////////////////////////////////////////
        var newMd5 = _md5Service.GetMd5(tempCsvPath);

        // 3️⃣ pobierz ostatni wpis
        var last = await _csvDateService.GetLastAsync();

        string finalFileName;
        string finalCsvPath;

        if (last != null)
        {
            var lastUrl = _gitHubService.GetRawUrl($"ceny/{last.FileName}");
            var lastMd5 = await _md5Service.GetMd5FromUrlAsync(lastUrl);

            if (newMd5 == lastMd5)
            {
                // 🔁 ten sam plik → reuse
                finalFileName = last.FileName;
            }
            else
            {
                // 🆕 nowa wersja
                var nextNumber = GetNextVersionNumber(last.FileName);
                finalFileName = $"cennik_{nextNumber}.csv";

                //finalCsvPath = Path.Combine(_env.WebRootPath, folderName, finalFileName);
                //tempCsvPath, finalCsvPath, true);

                var repoPath = $"ceny/{finalFileName}";
                await _gitHubService.UploadFileAsync(tempCsvPath, repoPath);
            }
        }
        else
        {
            // pierwszy plik ever
            finalFileName = "cennik_1.csv";
            finalCsvPath = Path.Combine(_env.WebRootPath, folderName, finalFileName);
            File.Copy(tempCsvPath, finalCsvPath, true);
        }

        // 6️⃣ zapis do DB
        await _csvDateService.AddAsync(today, finalFileName);

        // 7️⃣ XML
        var baseUrl = "https://raw.githubusercontent.com/Badym/DevEstate/main/ceny";

        var resource = new XmlResourceInfoDto
        {
            CsvUrl = baseUrl,
            DataDate = today
        };

        var outputDirFinal = Path.Combine(_env.WebRootPath, folderName);
        var xmlPath = await _xmlService.GenerateXml_3(resource, outputDirFinal);

        _md5Service.SaveMd5File(xmlPath);

        return (
            baseUrl,
            $"{_company.Website}/{folderName}/cennik.xml",
            $"{_company.Website}/{folderName}/cennik.md5",
            xmlPath
        );
    }
    
    private int GetNextVersionNumber(string fileName)
    {
        // cennik_3.csv → 3
        var numberPart = fileName
            .Replace("cennik_", "")
            .Replace(".csv", "");

        return int.Parse(numberPart) + 1;
    }
}