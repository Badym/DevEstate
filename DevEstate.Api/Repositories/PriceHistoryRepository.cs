using DevEstate.Api.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace DevEstate.Api.Repositories;

public class PriceHistoryRepository
{
    private readonly IMongoCollection<PriceHistory> _priceHistories;

    public PriceHistoryRepository(IMongoClient mongoClient, IOptions<MongoDbSettings> mongoSettings)
    {
        var database = mongoClient.GetDatabase(mongoSettings.Value.DatabaseName);
        _priceHistories = database.GetCollection<PriceHistory>("PriceHistories");
    }

    public async Task CreateAsync(PriceHistory priceHistory)
    {
        await _priceHistories.InsertOneAsync(priceHistory);
    }

    public async Task<PriceHistory> GetByIdAsync(string id)
    {
        return await _priceHistories.Find(ph => ph.Id == id).FirstOrDefaultAsync();
    }

    public async Task<List<PriceHistory>> GetAllAsync()
    {
        return await _priceHistories.Find(_ => true).ToListAsync();
    }

    public async Task DeleteAsync(string id)
    {
        await _priceHistories.DeleteOneAsync(ph => ph.Id == id);
    }
}
