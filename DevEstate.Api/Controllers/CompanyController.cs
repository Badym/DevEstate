using DevEstate.Api.Dtos;
using DevEstate.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace DevEstate.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CompanyController : ControllerBase
{
    private readonly CompanyService _service;

    public CompanyController(CompanyService service)
    {
        _service = service;
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id) => Ok(await _service.GetByIdAsync(id));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CompanyDtos.CompanyCreateDtos dto)
    {
        await _service.CreateAsync(dto);
        return Ok();
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] CompanyDtos.CompanyUpdateDtos dto)
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
    
    [HttpGet]
    public async Task<IActionResult> GetCompany()
    {
        var company = await _service.GetFirstAsync();
        if (company == null)
            return NotFound("No company data found.");
        Console.WriteLine($"Company found:asasasa");
    
        return Ok(company);
    }

}