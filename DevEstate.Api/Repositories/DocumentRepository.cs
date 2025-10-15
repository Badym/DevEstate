using DevEstate.Api.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace DevEstate.Api.Repositories;

public class DocumentRepository
{
    private readonly IMongoCollection<Document> _documents;

    public DocumentRepository(IMongoClient mongoClient, IOptions<MongoDbSettings> mongoSettings)
    {
        var database = mongoClient.GetDatabase(mongoSettings.Value.DatabaseName);
        _documents = database.GetCollection<Document>("Documents");
    }

    public async Task CreateAsync(Document document)
    {
        await _documents.InsertOneAsync(document);
    }

    public async Task<Document> GetByIdAsync(string id)
    {
        return await _documents.Find(d => d.Id == id).FirstOrDefaultAsync();
    }

    public async Task<List<Document>> GetAllAsync()
    {
        return await _documents.Find(_ => true).ToListAsync();
    }

    public async Task DeleteAsync(string id)
    {
        await _documents.DeleteOneAsync(d => d.Id == id);
    }
}
