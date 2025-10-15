using DevEstate.Api.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace DevEstate.Api.Repositories;

public class CompanyRepository
{
    private readonly IMongoCollection<Company> _companies;

    public CompanyRepository(IMongoClient mongoClient, IOptions<MongoDbSettings> mongoSettings)
    {
        var database = mongoClient.GetDatabase(mongoSettings.Value.DatabaseName);
        _companies = database.GetCollection<Company>("Companies");
    }

    public async Task CreateAsync(Company company)
    {
        await _companies.InsertOneAsync(company);
    }

    public async Task<Company> GetByIdAsync(string id)
    {
        return await _companies.Find(c => c.Id == id).FirstOrDefaultAsync();
    }

    public async Task<List<Company>> GetAllAsync()
    {
        return await _companies.Find(_ => true).ToListAsync();
    }

    public async Task UpdateAsync(Company company)
    {
        await _companies.ReplaceOneAsync(c => c.Id == company.Id, company);
    }

    public async Task DeleteAsync(string id)
    {
        await _companies.DeleteOneAsync(c => c.Id == id);
    }
}
