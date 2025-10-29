using DevEstate.Api.Dtos;
using DevEstate.Api.Models;
using DevEstate.Api.Repositories;

namespace DevEstate.Api.Services
{
    public class DocumentService
    {
        private readonly DocumentRepository _repo;

        public DocumentService(DocumentRepository repo)
        {
            _repo = repo;
        }

        public async Task<DocumentDtos.DocumentResponseDtos> GetByIdAsync(string id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) throw new Exception("Document not found");

            return MapToResponse(entity);
        }

        public async Task<List<DocumentDtos.DocumentResponseDtos>> GetAllAsync()
        {
            var entities = await _repo.GetAllAsync();
            return entities.Select(MapToResponse).ToList();
        }

        public async Task<List<DocumentDtos.DocumentResponseDtos>> GetByEntityAsync(string entityType, string entityId)
        {
            var entities = await _repo.GetByEntityAsync(entityType, entityId);
            return entities.Select(MapToResponse).ToList();
        }

        public async Task CreateAsync(DocumentDtos.DocumentCreateDtos dto)
        {
            var entity = new Document
            {
                InvestmentId = dto.InvestmentId,
                BuildingId = dto.BuildingId,
                PropertyId = dto.PropertyId,
                FileName = dto.FileName,
                FileType = dto.FileType,
                FileUrl = dto.FileUrl,
                Type = dto.Type,
                UploadedAt = DateTime.UtcNow
            };

            await _repo.CreateAsync(entity);
        }

        public async Task DeleteAsync(string id)
        {
            await _repo.DeleteAsync(id);
        }

        // 🔹 Prywatny mapper (czystość kodu)
        private static DocumentDtos.DocumentResponseDtos MapToResponse(Document e) => new()
        {
            Id = e.Id!,
            InvestmentId = e.InvestmentId,
            BuildingId = e.BuildingId,
            PropertyId = e.PropertyId,
            FileName = e.FileName,
            FileType = e.FileType,
            FileUrl = e.FileUrl,
            Type = e.Type,
            UploadedAt = e.UploadedAt
        };
    }
}