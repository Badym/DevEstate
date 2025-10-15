using DevEstate.Api.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace DevEstate.Api.Repositories;

public class InvestmentRepository
{
    private readonly IMongoCollection<Investment> _investments;

    public InvestmentRepository(IMongoClient mongoClient, IOptions<MongoDbSettings> mongoSettings)
    {
        var database = mongoClient.GetDatabase(mongoSettings.Value.DatabaseName);
        _investments = database.GetCollection<Investment>("Investments");
    }

    public async Task CreateAsync(Investment investment)
    {
        await _investments.InsertOneAsync(investment);
    }

    public async Task<Investment> GetByIdAsync(string id)
    {
        return await _investments.Find(i => i.Id == id).FirstOrDefaultAsync();
    }

    public async Task<List<Investment>> GetAllAsync()
    {
        return await _investments.Find(_ => true).ToListAsync();
    }

    public async Task UpdateAsync(Investment investment)
    {
        await _investments.ReplaceOneAsync(i => i.Id == investment.Id, investment);
    }

    public async Task DeleteAsync(string id)
    {
        await _investments.DeleteOneAsync(i => i.Id == id);
    }
}
