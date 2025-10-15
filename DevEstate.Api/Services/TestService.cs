using DevEstate.Api.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace DevEstate.Api.Services
{
    public class TestService
    { 
        private readonly IMongoCollection<Test> _tests;

        public TestService(IOptions<MongoDbSettings> mongoSettings, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(mongoSettings.Value.DatabaseName);
            _tests = database.GetCollection<Test>("tests");
        }

        public async Task<List<Test>> GetAllAsync() =>
            await _tests.Find(_ => true).ToListAsync();

        public async Task CreateAsync(Test test) =>
            await _tests.InsertOneAsync(test);
    }
}