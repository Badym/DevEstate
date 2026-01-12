using DevEstate.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevEstate.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProspectReportController : ControllerBase
    {
        private readonly ProspectReportService _service;

        public ProspectReportController(ProspectReportService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetReport()
        {
            var report = await _service.GenerateReportAsync();
            return Ok(report);
        }
        
        [HttpGet("download-csv")]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> DownloadCsvReport()
        {
            // Generowanie raportu CSV i uzyskanie pełnej ścieżki
            string filePath = await _service.GenerateCsvReportAsync();

            if (!System.IO.File.Exists(filePath))
                return NotFound("Plik CSV nie został znaleziony.");

            var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);

            return File(fileBytes, "text/csv", Path.GetFileName(filePath));
        }
    }
}