using DevEstate.Api.Dtos;
using DevEstate.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevEstate.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BuildingController : ControllerBase
{
    private readonly BuildingService _service;

    public BuildingController(BuildingService service)
    {
        _service = service;
        
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id) => Ok(await _service.GetByIdAsync(id));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] BuildingDtos.BuildingCreateDtos dto)
    {
        var fullName = User.FindFirst("fullName")?.Value;

        await _service.CreateAsync(dto, fullName);
        return Ok();
    }

    [HttpPatch("{id}")]
    [Authorize(Roles = "Admin,Moderator")]
    public async Task<IActionResult> Update(string id, [FromBody] BuildingDtos.BuildingUpdateDtos dto)
    {
        var fullName = User.FindFirst("fullName")?.Value;
        await _service.UpdateAsync(id, dto,fullName);
        return Ok();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin,Moderator")]
    public async Task<IActionResult> Delete(string id)
    {
        var fullName = User.FindFirst("fullName")?.Value;

        await _service.DeleteAsync(id, fullName);
        return Ok();
    }
}