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

        public async Task<DocumentDtos.Response> GetByIdAsync(string id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) throw new Exception("Document not found");

            return new DocumentDtos.Response
            {
                Id = entity.Id,
                PropertyId = entity.PropertyId,
                BuildingId = entity.BuildingId,
                FileName = entity.FileName,
                FileType = entity.FileType,
                FileUrl = entity.FileUrl,
                UploadedAt = entity.UploadedAt
            };
        }

        public async Task<List<DocumentDtos.Response>> GetAllAsync()
        {
            var entities = await _repo.GetAllAsync();
            return entities.Select(e => new DocumentDtos.Response
            {
                Id = e.Id,
                PropertyId = e.PropertyId,
                BuildingId = e.BuildingId,
                FileName = e.FileName,
                FileType = e.FileType,
                FileUrl = e.FileUrl,
                UploadedAt = e.UploadedAt
            }).ToList();
        }

        public async Task CreateAsync(DocumentDtos.Create dto)
        {
            var entity = new Document
            {
                PropertyId = dto.PropertyId,
                BuildingId = dto.BuildingId,
                FileName = dto.FileName,
                FileType = dto.FileType,
                FileUrl = dto.FileUrl
            };
            await _repo.CreateAsync(entity);
        }

        public async Task DeleteAsync(string id)
        {
            await _repo.DeleteAsync(id);
        }
    }
}