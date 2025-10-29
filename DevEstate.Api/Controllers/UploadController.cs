using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using DevEstate.Api.Services;

namespace DevEstate.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UploadController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        private readonly PropertyService _propertyService;
        private readonly BuildingService _buildingService;
        private readonly InvestmentService _investmentService;

        public UploadController(
            IWebHostEnvironment env,
            PropertyService propertyService,
            BuildingService buildingService,
            InvestmentService investmentService)
        {
            _env = env;
            _propertyService = propertyService;
            _buildingService = buildingService;
            _investmentService = investmentService;
        }

        [HttpPost("{entityType}/{entityId}")]
        public async Task<IActionResult> UploadImage(string entityType, string entityId, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Nie wybrano pliku.");

            var uploadsDir = Path.Combine(_env.ContentRootPath, "Uploads", "Images");
            if (!Directory.Exists(uploadsDir))
                Directory.CreateDirectory(uploadsDir);

            var fileName = $"{entityType}_{entityId}_{Path.GetRandomFileName()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(uploadsDir, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var fileUrl = $"{Request.Scheme}://{Request.Host}/uploads/{fileName}";

            switch (entityType.ToLower())
            {
                case "property":
                    await _propertyService.AddImageAsync(entityId, fileUrl);
                    break;
                case "building":
                    await _buildingService.AddImageAsync(entityId, fileUrl);
                    break;
                case "investment":
                    await _investmentService.AddImageAsync(entityId, fileUrl);
                    break;
                default:
                    return BadRequest("Nieprawidłowy typ encji.");
            }

            return Ok(new { fileName, fileUrl });
        }
    }
}
