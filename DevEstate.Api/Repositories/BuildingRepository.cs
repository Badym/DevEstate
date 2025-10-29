using DevEstate.Api.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace DevEstate.Api.Repositories;

public class BuildingRepository
{
    private readonly IMongoCollection<Building> _buildings;

    public BuildingRepository(IMongoClient mongoClient, IOptions<MongoDbSettings> mongoSettings)
    {
        var database = mongoClient.GetDatabase(mongoSettings.Value.DatabaseName);
        _buildings = database.GetCollection<Building>("Buildings");
    }

    public async Task CreateAsync(Building building)
    {
        await _buildings.InsertOneAsync(building);
    }

    public async Task<Building> GetByIdAsync(string id)
    {
        return await _buildings.Find(b => b.Id == id).FirstOrDefaultAsync();
    }

    public async Task<List<Building>> GetAllAsync()
    {
        return await _buildings.Find(_ => true).ToListAsync();
    }

    public async Task UpdateAsync(Building building)
    {
        await _buildings.ReplaceOneAsync(b => b.Id == building.Id, building);
    }

    public async Task DeleteAsync(string id)
    {
        await _buildings.DeleteOneAsync(b => b.Id == id);
    }
    
    public async Task<List<Building>> GetByInvestmentIdAsync(string investmentId)
    {
        var filter = Builders<Building>.Filter.Eq(b => b.InvestmentId, investmentId);
        return await _buildings.Find(filter).ToListAsync();
    }
}
