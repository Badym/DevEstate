using DevEstate.Api.Models;
using DevEstate.Api.Repositories;

namespace DevEstate.Api.Services;

public class AdminLogService
{
    private readonly AdminLogRepository _repo;

    public AdminLogService(AdminLogRepository repo)
    {
        _repo = repo;
    }

    public async Task LogAsync(string userFullName, string action, string entity, string entityId)
    {
        var log = new AdminLog
        {
            UserName = userFullName,
            Action = action,
            Entity = entity,
            EntityId = entityId,
            Timestamp = DateTime.UtcNow
        };

        await _repo.CreateAsync(log);
    }

    public async Task<List<AdminLog>> GetAllAsync()
        => await _repo.GetAllAsync();
    
    public async Task<List<AdminLog>> GetFilteredAsync(string? user, string? action, string? entity)
    {
        var logs = await _repo.GetAllAsync();

        if (!string.IsNullOrEmpty(user))
            logs = logs.Where(l => l.UserName.Contains(user, StringComparison.OrdinalIgnoreCase)).ToList();

        if (!string.IsNullOrEmpty(action))
            logs = logs.Where(l => l.Action.Equals(action, StringComparison.OrdinalIgnoreCase)).ToList();

        if (!string.IsNullOrEmpty(entity))
            logs = logs.Where(l => l.Entity.Equals(entity, StringComparison.OrdinalIgnoreCase)).ToList();

        return logs;
    }

}