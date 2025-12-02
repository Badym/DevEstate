using DevEstate.Api.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace DevEstate.Api.Repositories;

public class AdminLogRepository
{
    private readonly IMongoCollection<AdminLog> _collection;

    public AdminLogRepository(IMongoClient mongoClient, IOptions<MongoDbSettings> mongoSettings)
    {
        var database = mongoClient.GetDatabase(mongoSettings.Value.DatabaseName);
        _collection = database.GetCollection<AdminLog>("AdminLogs");
    }

    public async Task CreateAsync(AdminLog log)
    {
        await _collection.InsertOneAsync(log);
    }

    public async Task<List<AdminLog>> GetAllAsync()
    {
        return await _collection.Find(_ => true).ToListAsync();
    }
}