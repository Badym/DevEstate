using DevEstate.Api.Dtos;
using DevEstate.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace DevEstate.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FeatureController : ControllerBase
    {
        private readonly FeatureService _service;

        public FeatureController(FeatureService service)
        {
            _service = service;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var result = await _service.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpGet("byBuilding/{buildingId}")]
        public async Task<IActionResult> GetByBuilding(string buildingId)
        {
            var result = await _service.GetByBuildingIdAsync(buildingId);
            return Ok(result);
        }

        [HttpGet("byInvestment/{investmentId}")]
        public async Task<IActionResult> GetByInvestment(string investmentId)
        {
            var result = await _service.GetByInvestmentIdAsync(investmentId);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] FeatureDtos.FeatureCreateDtos dto)
        {
            var fullName = User.FindFirst("fullName")?.Value ?? "Unknown User";
            await _service.CreateAsync(dto,fullName);
            return Ok("Feature created successfully");
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] FeatureDtos.FeatureUpdateDtos dto)
        {
            var fullName = User.FindFirst("fullName")?.Value ?? "Unknown User";
            await _service.UpdateAsync(id, dto,fullName);
            return Ok("Feature updated successfully");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var fullName = User.FindFirst("fullName")?.Value ?? "Unknown User";
            await _service.DeleteAsync(id,fullName);
            return Ok("Feature deleted successfully");
        }
        
        [HttpGet("byBuilding/{buildingId}/types")]
        public async Task<IActionResult> GetFeatureTypesByBuilding(string buildingId)
            => Ok(await _service.GetFeatureTypesByBuildingAsync(buildingId));

        [HttpGet("byInvestment/{investmentId}/types")]
        public async Task<IActionResult> GetFeatureTypesByInvestment(string investmentId)
            => Ok(await _service.GetFeatureTypesByInvestmentAsync(investmentId));
        
        // GET /api/Feature/byBuilding/{buildingId}/type/{featureTypeId}
        [HttpGet("byBuilding/{buildingId}/type/{featureTypeId}")]
        public async Task<IActionResult> GetByBuildingAndType(string buildingId, string featureTypeId)
            => Ok(await _service.GetByBuildingAndTypeAsync(buildingId, featureTypeId));

// GET /api/Feature/byInvestment/{investmentId}/type/{featureTypeId}
        [HttpGet("byInvestment/{investmentId}/type/{featureTypeId}")]
        public async Task<IActionResult> GetByInvestmentAndType(string investmentId, string featureTypeId)
            => Ok(await _service.GetByInvestmentAndTypeAsync(investmentId, featureTypeId));

    }
}