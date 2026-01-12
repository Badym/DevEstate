using DevEstate.Api.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevEstate.Api.Controllers;

[ApiController]
    [Route("api/[controller]")]
    public class ImageController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        private readonly InvestmentRepository _investmentRepo;
        private readonly BuildingRepository _buildingRepo;
        private readonly PropertyRepository _propertyRepo;

        public ImageController(
            IWebHostEnvironment env,
            InvestmentRepository investmentRepo,
            BuildingRepository buildingRepo,
            PropertyRepository propertyRepo)
        {
            _env = env;
            _investmentRepo = investmentRepo;
            _buildingRepo = buildingRepo;
            _propertyRepo = propertyRepo;
        }

        /// <summary>
        /// Upload zdjęcia (JPG/PNG/WEBP) dla inwestycji, budynku lub mieszkania.
        /// </summary>
        [HttpPost("{entityType}/{entityId}")]
        [Consumes("multipart/form-data")]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> UploadImage(string entityType, string entityId, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Nie wybrano pliku.");

            var allowedTypes = new[] { "image/jpeg", "image/png", "image/webp" };
            if (!allowedTypes.Contains(file.ContentType))
                return BadRequest("Dozwolone tylko pliki JPG, PNG lub WEBP.");
            
            var uploadsDir = Path.Combine(_env.ContentRootPath, "Uploads", "Images");
            if (!Directory.Exists(uploadsDir))
                Directory.CreateDirectory(uploadsDir);

            var extension = Path.GetExtension(file.FileName);
            var uniqueName = $"{entityType}_{entityId}_{Guid.NewGuid():N}{extension}";
            var filePath = Path.Combine(uploadsDir, uniqueName);
            
            await using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            
            var fileUrl = $"/uploads/images/{uniqueName}";

            
            switch (entityType.ToLower())
            {
                case "investment":
                    var investment = await _investmentRepo.GetByIdAsync(entityId);
                    if (investment == null) return NotFound("Nie znaleziono inwestycji.");
                    investment.Images ??= new List<string>();
                    investment.Images.Add(fileUrl);
                    await _investmentRepo.UpdateAsync(investment);
                    break;

                case "building":
                    var building = await _buildingRepo.GetByIdAsync(entityId);
                    if (building == null) return NotFound("Nie znaleziono budynku.");
                    building.Images ??= new List<string>();
                    building.Images.Add(fileUrl);
                    await _buildingRepo.UpdateAsync(building);
                    break;

                case "property":
                    var property = await _propertyRepo.GetByIdAsync(entityId);
                    if (property == null) return NotFound("Nie znaleziono mieszkania / domu.");
                    property.Images ??= new List<string>();
                    property.Images.Add(fileUrl);
                    await _propertyRepo.UpdateAsync(property);
                    break;

                default:
                    return BadRequest("Nieprawidłowy typ encji. Dozwolone: investment / building / property.");
            }

            return Ok(new
            {
                message = "Zdjęcie zostało pomyślnie zapisane.",
                fileUrl
            });
        }
        
        
        [HttpDelete("{entityType}/{entityId}")]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> DeleteImage(string entityType, string entityId, [FromQuery] string imageUrl)
        {
            if (string.IsNullOrWhiteSpace(imageUrl))
                return BadRequest("Nie podano adresu zdjęcia.");

            var uploadsDir = Path.Combine(_env.ContentRootPath, "Uploads", "Images");
            var fileName = Path.GetFileName(imageUrl); // wyciąga nazwę pliku z URL
            var filePath = Path.Combine(uploadsDir, fileName);

            // 🗑️ Usuń plik z dysku (jeśli istnieje)
            if (System.IO.File.Exists(filePath))
                System.IO.File.Delete(filePath);

            // 🧱 Usuń link z bazy
            switch (entityType.ToLower())
            {
                case "investment":
                    var investment = await _investmentRepo.GetByIdAsync(entityId);
                    if (investment == null) return NotFound("Nie znaleziono inwestycji.");
                    investment.Images.RemoveAll(i => i == imageUrl);
                    await _investmentRepo.UpdateAsync(investment);
                    break;

                case "building":
                    var building = await _buildingRepo.GetByIdAsync(entityId);
                    if (building == null) return NotFound("Nie znaleziono budynku.");
                    building.Images.RemoveAll(i => i == imageUrl);
                    await _buildingRepo.UpdateAsync(building);
                    break;

                case "property":
                    var property = await _propertyRepo.GetByIdAsync(entityId);
                    if (property == null) return NotFound("Nie znaleziono mieszkania / domu.");
                    property.Images.RemoveAll(i => i == imageUrl);
                    await _propertyRepo.UpdateAsync(property);
                    break;

                default:
                    return BadRequest("Nieprawidłowy typ encji. Dozwolone: investment / building / property.");
            }

            return Ok(new
            {
                message = "Zdjęcie zostało pomyślnie usunięte.",
                deletedFile = fileName
            });
        }
    }