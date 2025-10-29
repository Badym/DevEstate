using DevEstate.Api.Dtos;
using DevEstate.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace DevEstate.Api.Controllers;

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
    public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id) => Ok(await _service.GetByIdAsync(id));

    [HttpGet("byBuilding/{buildingId}")]
    public async Task<IActionResult> GetByBuildingId(string buildingId)
        => Ok(await _service.GetByBuildingIdAsync(buildingId));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] FeatureDtos.FeatureCreateDtos dto)
    {
        await _service.CreateAsync(dto);
        return Ok();
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] FeatureDtos.FeatureUpdateDtos dto)
    {
        await _service.UpdateAsync(id, dto);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        await _service.DeleteAsync(id);
        return Ok();
    }
}