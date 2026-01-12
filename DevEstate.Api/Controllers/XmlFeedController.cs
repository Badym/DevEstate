using DevEstate.Api.Dtos;
using DevEstate.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevEstate.Api.Controllers;

[ApiController]
[Route("api/xml")]
public class XmlFeedController : ControllerBase
{
    private readonly ProspectReportService _reportService;
    private readonly Md5Service _md5Service;
    private readonly XmlPriceFeedService _xmlService;
    private readonly IWebHostEnvironment _env;
    private readonly CompanyDtos.CompanyDto _company;

    public XmlFeedController(
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

    [HttpPost("generate")]
    [Authorize(Roles = "Admin,Moderator")]
    public async Task<IActionResult> Generate()
    {
        string csvPath = await _reportService.GenerateCsvReportAsync();
        _md5Service.SaveMd5File(csvPath);

        string csvUrl = $"{_company.Website}/dane/cennik.csv";

        var resource = new XmlResourceInfoDto
        {
            CsvUrl = csvUrl,
            DataDate = DateTime.Today
        };

        string outputDir = Path.Combine(_env.WebRootPath, "dane");
        _xmlService.GenerateXml(resource, outputDir);

        return Ok(new
        {
            csv = csvUrl,
            md5 = csvUrl + ".md5",
            xml = $"{_company.Website}/dane/cennik.xml"
        });
    }
}