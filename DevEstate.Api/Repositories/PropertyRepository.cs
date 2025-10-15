using DevEstate.Api.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace DevEstate.Api.Repositories;

public class PropertyRepository
{
    private readonly IMongoCollection<Property> _properties;

    public PropertyRepository(IMongoClient mongoClient, IOptions<MongoDbSettings> mongoSettings)
    {
        var database = mongoClient.GetDatabase(mongoSettings.Value.DatabaseName);
        _properties = database.GetCollection<Property>("Properties");
    }

    public async Task CreateAsync(Property property)
    {
        await _properties.InsertOneAsync(property);
    }

    public async Task<Property> GetByIdAsync(string id)
    {
        return await _properties.Find(p => p.Id == id).FirstOrDefaultAsync();
    }

    public async Task<List<Property>> GetAllAsync()
    {
        return await _properties.Find(_ => true).ToListAsync();
    }

    public async Task UpdateAsync(Property property)
    {
        await _properties.ReplaceOneAsync(p => p.Id == property.Id, property);
    }

    public async Task DeleteAsync(string id)
    {
        await _properties.DeleteOneAsync(p => p.Id == id);
    }
}
