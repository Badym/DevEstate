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
        _documents = database.GetCollection<Document>("documents");
    }

    public async Task CreateAsync(Document document)
    {
        await _documents.InsertOneAsync(document);
    }

    public async Task<Document?> GetByIdAsync(string id)
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

    // 🔹 Zwraca dokumenty przypisane do konkretnej encji
    public async Task<List<Document>> GetByEntityAsync(string entityType, string entityId)
    {
        FilterDefinition<Document> filter = entityType.ToLower() switch
        {
            "investment" => Builders<Document>.Filter.Eq(d => d.InvestmentId, entityId),
            "building"   => Builders<Document>.Filter.Eq(d => d.BuildingId, entityId),
            "property"   => Builders<Document>.Filter.Eq(d => d.PropertyId, entityId),
            _            => Builders<Document>.Filter.Empty
        };

        return await _documents.Find(filter).ToListAsync();
    }
    
    public async Task DeleteByParentAsync(string parentType, string parentId)
    {
        FilterDefinition<Document> filter = parentType.ToLower() switch
        {
            "investment" => Builders<Document>.Filter.Eq(d => d.InvestmentId, parentId),
            "building" => Builders<Document>.Filter.Eq(d => d.BuildingId, parentId),
            "property" => Builders<Document>.Filter.Eq(d => d.PropertyId, parentId),
            _ => throw new ArgumentException($"Nieznany typ nadrzędny: {parentType}")
        };

        await _documents.DeleteManyAsync(filter);
    }
}
