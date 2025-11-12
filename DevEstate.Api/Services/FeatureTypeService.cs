using DevEstate.Api.Dtos;
using DevEstate.Api.Models;
using DevEstate.Api.Repositories;

namespace DevEstate.Api.Services
{
    public class FeatureTypeService
    {
        private readonly FeatureTypeRepository _repo;

        public FeatureTypeService(FeatureTypeRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<FeatureTypeDtos.FeatureTypeResponseDtos>> GetAllAsync()
        {
            var entities = await _repo.GetAllAsync();

            return entities.Select(e => new FeatureTypeDtos.FeatureTypeResponseDtos
            {
                Id = e.Id,
                Name = e.Name,
                UnitName = e.UnitName,
                IsActive = e.IsActive
            }).ToList();
        }

        public async Task<FeatureTypeDtos.FeatureTypeResponseDtos> GetByIdAsync(string id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null)
                throw new Exception("FeatureType not found");

            return new FeatureTypeDtos.FeatureTypeResponseDtos
            {
                Id = entity.Id,
                Name = entity.Name,
                UnitName = entity.UnitName,
                IsActive = entity.IsActive
            };
        }

        public async Task CreateAsync(FeatureTypeDtos.FeatureTypeCreateDtos dto)
        {
            var entity = new FeatureType
            {
                Name = dto.Name,
                UnitName = dto.UnitName,
                IsActive = dto.IsActive
            };

            await _repo.CreateAsync(entity);
        }

        public async Task UpdateAsync(string id, FeatureTypeDtos.FeatureTypeUpdateDtos dto)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null)
                throw new Exception("FeatureType not found");

            entity.Name = dto.Name ?? entity.Name;
            entity.UnitName = dto.UnitName ?? entity.UnitName;
            entity.IsActive = dto.IsActive ?? entity.IsActive;

            await _repo.UpdateAsync(entity);
        }

        public async Task DeleteAsync(string id)
        {
            await _repo.DeleteAsync(id);
        }
    }
}
