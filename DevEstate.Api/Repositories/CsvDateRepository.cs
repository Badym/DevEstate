using MongoDB.Driver;
using DevEstate.Api.Models;
using Microsoft.Extensions.Options;

namespace DevEstate.Api.Repositories
{
    public class CsvDateRepository
    {
        private readonly IMongoCollection<CsvDate> _collection;

        public CsvDateRepository(IMongoClient mongoClient, IOptions<MongoDbSettings> mongoSettings)
        {
            var database = mongoClient.GetDatabase(mongoSettings.Value.DatabaseName);
            _collection = database.GetCollection<CsvDate>("CsvDates");
        }

        public async Task CreateAsync(CsvDate entity)
        {
            await _collection.InsertOneAsync(entity);
        }

        public async Task<List<CsvDate>> GetAllAsync()
        {
            return await _collection
                .Find(_ => true)
                .SortByDescending(x => x.Date)
                .ToListAsync();
        }

        public async Task<CsvDate?> GetByDateAsync(DateTime date)
        {
            return await _collection.Find(x => x.Date == date).FirstOrDefaultAsync();
        }

        public async Task<CsvDate?> GetLastAsync()
        {
            return await _collection
                .Find(_ => true)
                .SortByDescending(x => x.Date)
                .FirstOrDefaultAsync();
        }

        public async Task DeleteAsync(string id)
        {
            await _collection.DeleteOneAsync(x => x.Id == id);
        }

        public async Task DeleteByDateAsync(DateTime date)
        {
            await _collection.DeleteOneAsync(x => x.Date == date);
        }
    }
}