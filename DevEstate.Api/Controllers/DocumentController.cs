using DevEstate.Api.Dtos;
using DevEstate.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevEstate.Api.Controllers;

[ApiController]
    [Route("api/[controller]")]
    public class DocumentController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        private readonly DocumentService _documentService;

        public DocumentController(IWebHostEnvironment env, DocumentService documentService)
        {
            _env = env;
            _documentService = documentService;
        }

        // ------------------ GET ALL ------------------
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var documents = await _documentService.GetAllAsync();
            return Ok(documents);
        }

        // ------------------ GET BY ID ------------------
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var document = await _documentService.GetByIdAsync(id);
            return Ok(document);
        }

        // ------------------ DELETE ------------------
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> Delete(string id)
        {
            await _documentService.DeleteAsync(id);
            return NoContent();
        }


        [HttpPost("{entityType}/{entityId}")]
        [Consumes("multipart/form-data")]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> UploadDocument(string entityType, string entityId, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Nie wybrano pliku.");

            var allowedTypes = new[]
            {
                "application/pdf",
                "application/msword",
                "application/vnd.openxmlformats-officedocument.wordprocessingml.document"
            };

            if (!allowedTypes.Contains(file.ContentType))
                return BadRequest("Dozwolone tylko pliki PDF lub DOCX.");

            var uploadsDir = Path.Combine(_env.ContentRootPath, "Uploads", "Documents");
            if (!Directory.Exists(uploadsDir))
                Directory.CreateDirectory(uploadsDir);

            var extension = Path.GetExtension(file.FileName);
            var uniqueName = $"{entityType}_{entityId}_{Guid.NewGuid():N}{extension}";
            var filePath = Path.Combine(uploadsDir, uniqueName);

            await using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var fileUrl = $"{Request.Scheme}://{Request.Host}/uploads/documents/{uniqueName}";

            var dto = new DocumentDtos.DocumentCreateDtos()
            {
                FileName = file.FileName,
                FileType = file.ContentType,
                FileUrl = fileUrl
            };

            switch (entityType.ToLower())
            {
                case "investment":
                    dto.InvestmentId = entityId;
                    break;
                case "building":
                    dto.BuildingId = entityId;
                    break;
                case "property":
                    dto.PropertyId = entityId;
                    break;
                default:
                    return BadRequest("Nieprawidłowy typ encji. Dozwolone: investment / building / property.");
            }

            await _documentService.CreateAsync(dto);

            return Ok(new
            {
                message = "Plik został pomyślnie zapisany.",
                fileUrl,
                fileName = file.FileName,
                fileType = file.ContentType
            });
        }
        
        // ------------------ GET BY ENTITY ------------------
        [HttpGet("{entityType}/{entityId}")]
        public async Task<IActionResult> GetByEntity(string entityType, string entityId)
        {
            var allDocuments = await _documentService.GetAllAsync();

            List<DocumentDtos.DocumentResponseDtos> filtered = entityType.ToLower() switch
            {
                "investment" => allDocuments.Where(d => d.InvestmentId == entityId).ToList(),
                "building"   => allDocuments.Where(d => d.BuildingId == entityId).ToList(),
                "property"   => allDocuments.Where(d => d.PropertyId == entityId).ToList(),
                _ => new List<DocumentDtos.DocumentResponseDtos>()
            };

            //if (!filtered.Any())
            //    return NotFound($"Brak dokumentów dla {entityType} o ID: {entityId}");

            return Ok(filtered);
        }

    }