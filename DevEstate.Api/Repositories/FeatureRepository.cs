using MongoDB.Driver;
using DevEstate.Api.Models;
using Microsoft.Extensions.Options;

namespace DevEstate.Api.Repositories
{
    public class FeatureRepository
    {
        private readonly IMongoCollection<Feature> _features;

        public FeatureRepository(IMongoClient mongoClient, IOptions<MongoDbSettings> mongoSettings)
        {
            var database = mongoClient.GetDatabase(mongoSettings.Value.DatabaseName);
            _features = database.GetCollection<Feature>("Features");
        }

        // Tworzenie nowej cechy
        public async Task CreateAsync(Feature feature)
        {
            await _features.InsertOneAsync(feature);
        }

        // Pobranie cechy po ID
        public async Task<Feature> GetByIdAsync(string id)
        {
            return await _features.Find(f => f.Id == id).FirstOrDefaultAsync();
        }

        // Pobranie wszystkich cech
        public async Task<List<Feature>> GetAllAsync()
        {
            return await _features.Find(_ => true).ToListAsync();
        }

        // Aktualizacja cechy
        public async Task UpdateAsync(Feature feature)
        {
            await _features.ReplaceOneAsync(f => f.Id == feature.Id, feature);
        }

        // Usunięcie cechy
        public async Task DeleteAsync(string id)
        {
            await _features.DeleteOneAsync(f => f.Id == id);
        }

        // Pobranie wszystkich cech dla danego budynku
        public async Task<List<Feature>> GetByBuildingIdAsync(string buildingId)
        {
            return await _features.Find(f => f.BuildingId == buildingId).ToListAsync();
        }
    }
}