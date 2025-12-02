using DevEstate.Api.Models;
using DevEstate.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

public class DeveloperPriceRepository
{
    private readonly IMongoCollection<DeveloperPriceEntity> _collection;

    public DeveloperPriceRepository(IMongoClient client, IOptions<MongoDbSettings> settings)
    {
        var db = client.GetDatabase(settings.Value.DatabaseName);
        _collection = db.GetCollection<DeveloperPriceEntity>("DeveloperPrices");
    }

    public async Task<DeveloperPriceEntity?> GetByFullLocationAsync(string w, string p, string g, string m)
    {
        return await _collection
            .Find(x => x.Wojewodztwo == w && x.Powiat == p && x.Gmina == g && x.Miejscowosc == m)
            .FirstOrDefaultAsync();
    }

    public async Task<DeveloperPriceEntity?> GetByRegionAsync(string w, string p)
    {
        return await _collection
            .Find(x => x.Wojewodztwo == w && x.Powiat == p)
            .FirstOrDefaultAsync();
    }

    public async Task CreateAsync(DeveloperPriceEntity e)
    {
        await _collection.InsertOneAsync(e);
    }

    public async Task UpdateAsync(DeveloperPriceEntity e)
    {
        await _collection.ReplaceOneAsync(x => x.Id == e.Id, e);
    }
}