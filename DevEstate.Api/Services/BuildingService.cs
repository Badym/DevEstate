using DevEstate.Api.Dtos;
using DevEstate.Api.Models;
using DevEstate.Api.Repositories;

namespace DevEstate.Api.Services
{
    public class BuildingService
    {
        private readonly BuildingRepository _repo;

        public BuildingService(BuildingRepository repo)
        {
            _repo = repo;
        }

        public async Task<BuildingDtos.Response> GetByIdAsync(string id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) throw new Exception("Building not found");

            return new BuildingDtos.Response
            {
                Id = entity.Id,
                BuildingNumber = entity.BuildingNumber,
                Description = entity.Description,
                Status = entity.Status,
                CreatedAt = entity.CreatedAt
            };
        }

        public async Task<List<BuildingDtos.Response>> GetAllAsync()
        {
            var entities = await _repo.GetAllAsync();
            return entities.Select(e => new BuildingDtos.Response
            {
                Id = e.Id,
                BuildingNumber = e.BuildingNumber,
                Description = e.Description,
                Status = e.Status,
                CreatedAt = e.CreatedAt
            }).ToList();
        }

        public async Task CreateAsync(BuildingDtos.Create dto)
        {
            var entity = new Building
            {
                InvestmentId = dto.InvestmentId,
                BuildingNumber = dto.BuildingNumber,
                Description = dto.Description,
                Status = dto.Status
            };
            await _repo.CreateAsync(entity);
        }

        public async Task UpdateAsync(string id, BuildingDtos.Update dto)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) throw new Exception("Building not found");

            entity.BuildingNumber = dto.BuildingNumber ?? entity.BuildingNumber;
            entity.Description = dto.Description ?? entity.Description;
            entity.Status = dto.Status ?? entity.Status;

            await _repo.UpdateAsync(entity);
        }

        public async Task DeleteAsync(string id)
        {
            await _repo.DeleteAsync(id);
        }
        
        public async Task AddImageAsync(string buildingId, string fileUrl)
        {
            var building = await _repo.GetByIdAsync(buildingId);
            if (building != null)
            {
                building.Images ??= new List<string>();
                building.Images.Add(fileUrl);
                await _repo.UpdateAsync(building);
            }
        }

    }
}
