using DevEstate.Api.Dtos;
using DevEstate.Api.Models;
using DevEstate.Api.Repositories;

namespace DevEstate.Api.Services
{
    public class BuildingService
    {
        private readonly BuildingRepository _buildingRepo;
        private readonly PropertyService _propertyService;
        private readonly FeatureRepository _featureRepo;
        private readonly DocumentRepository _documentRepo;

        public BuildingService(
            BuildingRepository buildingRepo,
            PropertyService propertyService,
            FeatureRepository featureRepo,
            DocumentRepository documentRepo)
        {
            _buildingRepo = buildingRepo;
            _propertyService = propertyService;
            _featureRepo = featureRepo;
            _documentRepo = documentRepo;
        }

        public async Task<BuildingDtos.BuildingResponseDtos> GetByIdAsync(string id)
        {
            var entity = await _buildingRepo.GetByIdAsync(id);
            if (entity == null) throw new Exception("Building not found");

            return new BuildingDtos.BuildingResponseDtos()
            {
                Id = entity.Id,
                BuildingNumber = entity.BuildingNumber,
                Description = entity.Description,
                Status = entity.Status,
                Images = entity.Images.ToList(),
                CreatedAt = entity.CreatedAt,
                InvestmentId = entity.InvestmentId,
            };
        }

        public async Task<List<BuildingDtos.BuildingResponseDtos>> GetAllAsync()
        {
            var entities = await _buildingRepo.GetAllAsync();
            return entities.Select(e => new BuildingDtos.BuildingResponseDtos()
            {
                Id = e.Id,
                BuildingNumber = e.BuildingNumber,
                Description = e.Description,
                Status = e.Status,
                Images = e.Images.ToList(),
                CreatedAt = e.CreatedAt,
                InvestmentId = e.InvestmentId,
            }).ToList();
        }

        public async Task CreateAsync(BuildingDtos.BuildingCreateDtos dto)
        {
            var entity = new Building
            {
                InvestmentId = dto.InvestmentId,
                BuildingNumber = dto.BuildingNumber,
                Description = dto.Description,
                Status = dto.Status
            };
            await _buildingRepo.CreateAsync(entity);
        }

        public async Task UpdateAsync(string id, BuildingDtos.BuildingUpdateDtos dto)
        {
            var entity = await _buildingRepo.GetByIdAsync(id);
            if (entity == null) throw new Exception("Building not found");

            entity.BuildingNumber = dto.BuildingNumber ?? entity.BuildingNumber;
            entity.Description = dto.Description ?? entity.Description;
            entity.Status = dto.Status ?? entity.Status;

            await _buildingRepo.UpdateAsync(entity);
        }

        public async Task DeleteAsync(string id)
        {
            // usuń dokumenty budynku
            await _documentRepo.DeleteByParentAsync("building", id);

            // usuń features powiązane z budynkiem
            await _featureRepo.DeleteByBuildingIdAsync(id);

            // usuń wszystkie properties przypisane do budynku
            var properties = await _propertyService.GetByBuildingIdAsync(id);
            if (properties != null && properties.Any())
            {
                foreach (var property in properties)
                {
                    await _propertyService.DeleteAsync(property.Id);
                }
            }

            // usuń budynek
            await _buildingRepo.DeleteAsync(id);
        }

        
        public async Task AddImageAsync(string buildingId, string fileUrl)
        {
            var building = await _buildingRepo.GetByIdAsync(buildingId);
            if (building != null)
            {
                building.Images ??= new List<string>();
                building.Images.Add(fileUrl);
                await _buildingRepo.UpdateAsync(building);
            }
        }
        
        public async Task<IEnumerable<BuildingDtos.BuildingResponseDtos>> GetByInvestmentIdAsync(string investmentId)
        {
            var buildings = await _buildingRepo.GetByInvestmentIdAsync(investmentId);
            var result = new List<BuildingDtos.BuildingResponseDtos>();

            foreach (var b in buildings)
            {
                var (available, sold) = await GetBuildingStatisticsAsync(b.Id);

                result.Add(new BuildingDtos.BuildingResponseDtos
                {
                    Id = b.Id,
                    InvestmentId = b.InvestmentId,
                    BuildingNumber = b.BuildingNumber,
                    Description = b.Description,
                    Status = b.Status,
                    Images = b.Images.ToList(),
                    CreatedAt = b.CreatedAt,
                    AvailablePropertiesCount = available,
                    SoldPropertiesCount = sold
                });
            }

            return result;
        }
        
       
        public async Task<List<Building>> GetEntitiesByInvestmentIdAsync(string investmentId)
        {
            return await _buildingRepo.GetByInvestmentIdAsync(investmentId);
        }

        
        public async Task<(int available, int sold)> GetBuildingStatisticsAsync(string buildingId)
        {
            var properties = await _propertyService.GetByBuildingIdAsync(buildingId);

            int available = properties.Count(p =>
                p.Status.Equals("Aktualne", StringComparison.OrdinalIgnoreCase) ||
                p.Status.Equals("Na sprzedaż", StringComparison.OrdinalIgnoreCase));

            int sold = properties.Count(p =>
                p.Status.Equals("Sprzedane", StringComparison.OrdinalIgnoreCase));

            return (available, sold);
        }

    }
}
