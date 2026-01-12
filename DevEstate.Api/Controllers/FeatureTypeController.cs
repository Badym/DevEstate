using DevEstate.Api.Dtos;
using DevEstate.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevEstate.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FeatureTypeController : ControllerBase
    {
        private readonly FeatureTypeService _service;

        public FeatureTypeController(FeatureTypeService service)
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

        [HttpPost]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> Create([FromBody] FeatureTypeDtos.FeatureTypeCreateDtos dto)
        {
            await _service.CreateAsync(dto);
            return Ok("Feature type created successfully");
        }

        [HttpPatch("{id}")]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> Update(string id, [FromBody] FeatureTypeDtos.FeatureTypeUpdateDtos dto)
        {
            await _service.UpdateAsync(id, dto);
            return Ok("Feature type updated successfully");
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> Delete(string id)
        {
            await _service.DeleteAsync(id);
            return Ok("Feature type deleted successfully");
        }
    }
}