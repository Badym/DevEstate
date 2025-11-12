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

        public async Task CreateAsync(Feature feature)
        {
            await _features.InsertOneAsync(feature);
        }

        public async Task<Feature?> GetByIdAsync(string id)
        {
            return await _features.Find(f => f.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<Feature>> GetAllAsync()
        {
            return await _features.Find(_ => true).ToListAsync();
        }

        public async Task UpdateAsync(Feature feature)
        {
            await _features.ReplaceOneAsync(f => f.Id == feature.Id, feature);
        }

        public async Task DeleteAsync(string id)
        {
            await _features.DeleteOneAsync(f => f.Id == id);
        }

        public async Task<List<Feature>> GetByBuildingIdAsync(string buildingId)
        {
            return await _features.Find(f => f.BuildingId == buildingId).ToListAsync();
        }

        public async Task<List<Feature>> GetByInvestmentIdAsync(string investmentId)
        {
            return await _features.Find(f => f.InvestmentId == investmentId).ToListAsync();
        }

        public async Task DeleteByBuildingIdAsync(string buildingId)
        {
            var filter = Builders<Feature>.Filter.Eq(f => f.BuildingId, buildingId);
            await _features.DeleteManyAsync(filter);
        }

        public async Task DeleteByInvestmentIdAsync(string investmentId)
        {
            var filter = Builders<Feature>.Filter.Eq(f => f.InvestmentId, investmentId);
            await _features.DeleteManyAsync(filter);
        }
        
        
    }
}
