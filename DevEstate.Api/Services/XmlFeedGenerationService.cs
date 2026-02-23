using DevEstate.Api.Dtos;

namespace DevEstate.Api.Services;

public class XmlFeedGenerationService
{
    private readonly ProspectReportService _reportService;
    private readonly Md5Service _md5Service;
    private readonly XmlPriceFeedService _xmlService;
    private readonly IWebHostEnvironment _env;
    private readonly CompanyDtos.CompanyDto _company;

    public XmlFeedGenerationService(
        ProspectReportService reportService,
        Md5Service md5Service,
        XmlPriceFeedService xmlService,
        IWebHostEnvironment env,
        CompanyDtos.CompanyDto company)
    {
        _reportService = reportService;
        _md5Service = md5Service;
        _xmlService = xmlService;
        _env = env;
        _company = company;
    }

    public async Task<(string csvUrl, string xmlUrl, string md5Url, string xmlPath)> GenerateAsync(string folderName = "dane")
    {
        await _reportService.GenerateCsvReportAsync(folderName);

        var csvUrl = $"{_company.Website}/{folderName}/cennik.csv";

        var resource = new XmlResourceInfoDto
        {
            CsvUrl = csvUrl,
            DataDate = DateTime.Today
        };

        var outputDir = Path.Combine(_env.WebRootPath, folderName);
        var xmlPath = await _xmlService.GenerateXml(resource, outputDir);

        _md5Service.SaveMd5File(xmlPath);

        var xmlUrl = $"{_company.Website}/{folderName}/cennik.xml";
        var md5Url = $"{_company.Website}/{folderName}/cennik.md5";

        return (csvUrl, xmlUrl, md5Url, xmlPath);
    }
}