using DevEstate.Api.Dtos;
using DevEstate.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AdminLogController : ControllerBase
{
    private readonly AdminLogService _service;

    public AdminLogController(AdminLogService service)
    {
        _service = service;
    }

    [HttpGet]
    //[Authorize]  // tylko zalogowani mogą przeglądać logi
    public async Task<ActionResult<List<AdminLogDtos.AdminLogResponseDtos>>> GetAll()
    {
        var logs = await _service.GetAllAsync();

        var response = logs.Select(l => new AdminLogDtos.AdminLogResponseDtos
        {
            Id = l.Id,
            UserName = l.UserName,
            Action = l.Action,
            Entity = l.Entity,
            EntityId = l.EntityId,
            Timestamp = l.Timestamp
        }).ToList();

        return Ok(response);
    }

    // GET: api/admin-log/filter?user=...&action=...&entity=...
    [HttpGet("filter")]
    //[Authorize]
    public async Task<ActionResult<List<AdminLogDtos.AdminLogResponseDtos>>> GetFiltered(
        [FromQuery] AdminLogDtos.AdminLogFilterDtos filter)
    {
        var logs = await _service.GetAllAsync();

        if (!string.IsNullOrWhiteSpace(filter.UserName))
            logs = logs.Where(l => l.UserName.Contains(filter.UserName, StringComparison.OrdinalIgnoreCase)).ToList();

        if (!string.IsNullOrWhiteSpace(filter.Action))
            logs = logs.Where(l => l.Action.Equals(filter.Action, StringComparison.OrdinalIgnoreCase)).ToList();

        if (!string.IsNullOrWhiteSpace(filter.Entity))
            logs = logs.Where(l => l.Entity.Equals(filter.Entity, StringComparison.OrdinalIgnoreCase)).ToList();

        var response = logs.Select(l => new AdminLogDtos.AdminLogResponseDtos
        {
            Id = l.Id,
            UserName = l.UserName,
            Action = l.Action,
            Entity = l.Entity,
            EntityId = l.EntityId,
            Timestamp = l.Timestamp
        }).ToList();

        return Ok(response);
    }
}