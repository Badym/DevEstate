using MongoDB.Driver;
using DevEstate.Api.Models;
using Microsoft.Extensions.Options;

namespace DevEstate.Api.Repositories
{
    public class FeatureTypeRepository
    {
        private readonly IMongoCollection<FeatureType> _featureTypes;

        public FeatureTypeRepository(IMongoClient mongoClient, IOptions<MongoDbSettings> mongoSettings)
        {
            var database = mongoClient.GetDatabase(mongoSettings.Value.DatabaseName);
            _featureTypes = database.GetCollection<FeatureType>("FeatureTypes");
        }

        public async Task<List<FeatureType>> GetAllAsync()
        {
            return await _featureTypes.Find(_ => true).ToListAsync();
        }

        public async Task<FeatureType?> GetByIdAsync(string id)
        {
            return await _featureTypes.Find(ft => ft.Id == id).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(FeatureType featureType)
        {
            await _featureTypes.InsertOneAsync(featureType);
        }

        public async Task UpdateAsync(FeatureType featureType)
        {
            await _featureTypes.ReplaceOneAsync(ft => ft.Id == featureType.Id, featureType);
        }

        public async Task DeleteAsync(string id)
        {
            await _featureTypes.DeleteOneAsync(ft => ft.Id == id);
        }
    }
}