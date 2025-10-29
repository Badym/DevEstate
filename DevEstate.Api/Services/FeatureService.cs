using DevEstate.Api.Dtos;
using DevEstate.Api.Models;
using DevEstate.Api.Repositories;

namespace DevEstate.Api.Services
{
    public class FeatureService
    {
        private readonly FeatureRepository _repo;

        public FeatureService(FeatureRepository repo)
        {
            _repo = repo;
        }

        public async Task<FeatureDtos.FeatureResponseDtos> GetByIdAsync(string id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) throw new Exception("Feature not found");

            return new FeatureDtos.FeatureResponseDtos
            {
                Id = entity.Id,
                BuildingId = entity.BuildingId,
                Name = entity.Name,
                Description = entity.Description
            };
        }

        public async Task<List<FeatureDtos.FeatureResponseDtos>> GetAllAsync()
        {
            var entities = await _repo.GetAllAsync();
            return entities.Select(e => new FeatureDtos.FeatureResponseDtos
            {
                Id = e.Id,
                BuildingId = e.BuildingId,
                Name = e.Name,
                Description = e.Description
            }).ToList();
        }

        public async Task CreateAsync(FeatureDtos.FeatureCreateDtos dto)
        {
            var entity = new Feature
            {
                BuildingId = dto.BuildingId,
                Name = dto.Name,
                Description = dto.Description
            };
            await _repo.CreateAsync(entity);
        }

        public async Task UpdateAsync(string id, FeatureDtos.FeatureUpdateDtos dto)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) throw new Exception("Feature not found");

            entity.Name = dto.Name ?? entity.Name;
            entity.Description = dto.Description ?? entity.Description;
            entity.Price = dto.Price ?? entity.Price;

            await _repo.UpdateAsync(entity);
        }


        public async Task DeleteAsync(string id)
        {
            await _repo.DeleteAsync(id);
        }

        public async Task<List<FeatureDtos.FeatureResponseDtos>> GetByBuildingIdAsync(string buildingId)
        {
            var entities = await _repo.GetByBuildingIdAsync(buildingId);

            return entities.Select(e => new FeatureDtos.FeatureResponseDtos
            {
                Id = e.Id!,
                BuildingId = e.BuildingId,
                Name = e.Name,
                Description = e.Description,
                Price = e.Price
            }).ToList();
        }
    }
}
