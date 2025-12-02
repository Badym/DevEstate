using DevEstate.Api.Dtos;
using DevEstate.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace DevEstate.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InvestmentController : ControllerBase
{
    private readonly InvestmentService _service;
    private readonly BuildingService _buildingService;
    private readonly PropertyService _propertyService;

    public InvestmentController(InvestmentService service, BuildingService buildingService, PropertyService propertyService)
    {
        _service = service;
        _buildingService = buildingService;
        _propertyService = propertyService;
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id) => Ok(await _service.GetByIdAsync(id));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] InvestmentDtos.InvestmentCreateDtos dto)
    {
        var fullName = User.FindFirst("fullName")?.Value;
        await _service.CreateAsync(dto, fullName);
        return Ok();
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] InvestmentDtos.InvestmentUpdateDtos dto)
    {
        var fullName = User.FindFirst("fullName")?.Value;
        await _service.UpdateAsync(id, dto, fullName);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var fullName = User.FindFirst("fullName")?.Value;
        await _service.DeleteAsync(id, fullName);
        return Ok();
    }
    
    [HttpGet("{investmentId}/buildings")]
    public async Task<IActionResult> GetBuildingsByInvestmentId(string investmentId)
    {
        var buildings = await _buildingService.GetByInvestmentIdAsync(investmentId);

        if (!buildings.Any())
            return NotFound(new { message = "Brak budynków dla tej inwestycji." });

        return Ok(buildings);
    }
    
    [HttpGet("{investmentId}/properties")]
    public async Task<IActionResult> GetPropertiesByInvestmentId(string investmentId)
    {
        var properties = await _propertyService.GetByInvestmentIdAsync(investmentId);

        if (properties == null || !properties.Any())
            return NotFound(new { message = "Brak lokali (properties) dla tej inwestycji." });

        return Ok(properties);
    }

    [HttpGet("{investmentId}/properties-by-type")]
    public async Task<IActionResult> GetPropertiesByType(string investmentId, [FromQuery] string type)
    {
        if (string.IsNullOrWhiteSpace(type))
            return BadRequest("Parametr 'type' jest wymagany (apartment/house).");

        var properties = await _propertyService.GetByInvestmentIdAndTypeAsync(investmentId, type);

        if (properties == null || !properties.Any())
            return NotFound($"Nie znaleziono nieruchomości typu '{type}' dla inwestycji o ID: {investmentId}.");

        return Ok(properties);
    }
    
    [HttpGet("status/{status}")]
    public async Task<IActionResult> GetByStatus(string status)
    {
        if (string.IsNullOrWhiteSpace(status))
            return BadRequest("Status jest wymagany.");

        var validStatuses = new[] { "Aktualne", "Sprzedane" };

        if (!validStatuses.Contains(status))
            return BadRequest("Nieprawidłowy status. Dozwolone statusy to: 'Aktualne' i 'Sprzedane'.");

        var investments = await _service.GetByStatusAsync(status);

        if (!investments.Any())
            return NotFound($"Brak inwestycji o statusie '{status}'.");

        return Ok(investments);
    }

    [HttpGet("name/{name}")]
    public async Task<IActionResult> GetByName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return BadRequest("Name is required.");

        var investment = await _service.GetByNameAsync(name.Replace("_", " ")); // zamiana "_" → " "

        if (investment == null)
            return NotFound(new { message = $"Investment with name '{name}' not found." });

        return Ok(investment);
    }


}