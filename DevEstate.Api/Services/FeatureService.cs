using DevEstate.Api.Dtos;
using DevEstate.Api.Models;
using DevEstate.Api.Repositories;

namespace DevEstate.Api.Services
{
    public class FeatureService
    {
        private readonly FeatureRepository _repo;
        private readonly FeatureTypeRepository _featureTypeRepo;

        public FeatureService(FeatureRepository repo, FeatureTypeRepository featureTypeRepo)
        {
            _repo = repo;
            _featureTypeRepo = featureTypeRepo;
        }

        public async Task<FeatureDtos.FeatureResponseDtos> GetByIdAsync(string id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null)
                throw new Exception("Feature not found");

            return new FeatureDtos.FeatureResponseDtos
            {
                Id = entity.Id,
                InvestmentId = entity.InvestmentId,
                BuildingId = entity.BuildingId,
                FeatureTypeId = entity.FeatureTypeId,
                Price = entity.Price,
                IsAvailable = entity.IsAvailable,
                Description = entity.Description
            };
        }

        public async Task<List<FeatureDtos.FeatureResponseDtos>> GetAllAsync()
        {
            var entities = await _repo.GetAllAsync();
            return entities.Select(e => new FeatureDtos.FeatureResponseDtos
            {
                Id = e.Id,
                InvestmentId = e.InvestmentId,
                BuildingId = e.BuildingId,
                FeatureTypeId = e.FeatureTypeId,
                Price = e.Price,
                IsAvailable = e.IsAvailable,
                Description = e.Description
            }).ToList();
        }

        public async Task<List<FeatureDtos.FeatureResponseDtos>> GetByBuildingIdAsync(string buildingId)
        {
            var entities = await _repo.GetByBuildingIdAsync(buildingId);
            return entities.Select(e => new FeatureDtos.FeatureResponseDtos
            {
                Id = e.Id!,
                InvestmentId = e.InvestmentId,
                BuildingId = e.BuildingId,
                FeatureTypeId = e.FeatureTypeId,
                Price = e.Price,
                IsAvailable = e.IsAvailable,
                Description = e.Description
            }).ToList();
        }

        public async Task<List<FeatureDtos.FeatureResponseDtos>> GetByInvestmentIdAsync(string investmentId)
        {
            var entities = await _repo.GetByInvestmentIdAsync(investmentId);
            return entities.Select(e => new FeatureDtos.FeatureResponseDtos
            {
                Id = e.Id!,
                InvestmentId = e.InvestmentId,
                BuildingId = e.BuildingId,
                FeatureTypeId = e.FeatureTypeId,
                Price = e.Price,
                IsAvailable = e.IsAvailable,
                Description = e.Description
            }).ToList();
        }

        public async Task CreateAsync(FeatureDtos.FeatureCreateDtos dto)
        {
            var entity = new Feature
            {
                InvestmentId = dto.InvestmentId,
                BuildingId = dto.BuildingId,
                FeatureTypeId = dto.FeatureTypeId,
                Price = dto.Price,
                IsAvailable = dto.IsAvailable,
                Description = dto.Description
            };
            await _repo.CreateAsync(entity);
        }

        public async Task UpdateAsync(string id, FeatureDtos.FeatureUpdateDtos dto)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null)
                throw new Exception("Feature not found");

            entity.InvestmentId = dto.InvestmentId ?? entity.InvestmentId;
            entity.BuildingId = dto.BuildingId ?? entity.BuildingId;
            entity.FeatureTypeId = dto.FeatureTypeId ?? entity.FeatureTypeId;
            entity.Price = dto.Price ?? entity.Price;
            entity.IsAvailable = dto.IsAvailable ?? entity.IsAvailable;
            entity.Description = dto.Description ?? entity.Description;

            await _repo.UpdateAsync(entity);
        }

        public async Task DeleteAsync(string id)
        {
            await _repo.DeleteAsync(id);
        }
        
        public async Task<List<FeatureDtos.FeatureTypeSummaryDto>> GetFeatureTypesByBuildingAsync(string buildingId)
        {
            var features = await _repo.GetByBuildingIdAsync(buildingId);
            if (!features.Any()) return new List<FeatureDtos.FeatureTypeSummaryDto>();

            var types = await _featureTypeRepo.GetAllAsync();
            var grouped = features
                .GroupBy(f => f.FeatureTypeId)
                .Select(g => new FeatureDtos.FeatureTypeSummaryDto
                {
                    FeatureTypeId = g.Key,
                    FeatureTypeName = types.FirstOrDefault(t => t.Id == g.Key)?.Name ?? "(unknown)",
                    MinPrice = g.Min(f => f.Price),
                    MaxPrice = g.Max(f => f.Price),
                    Count = g.Count()
                })
                .ToList();

            return grouped;
        }

        public async Task<List<FeatureDtos.FeatureTypeSummaryDto>> GetFeatureTypesByInvestmentAsync(string investmentId)
        {
            var features = await _repo.GetByInvestmentIdAsync(investmentId);

            // tylko te, które nie są przypisane do konkretnego budynku
            var investmentFeatures = features.Where(f => f.BuildingId == null).ToList();
            if (!investmentFeatures.Any()) return new List<FeatureDtos.FeatureTypeSummaryDto>();

            var types = await _featureTypeRepo.GetAllAsync();
            var grouped = investmentFeatures
                .GroupBy(f => f.FeatureTypeId)
                .Select(g => new FeatureDtos.FeatureTypeSummaryDto
                {
                    FeatureTypeId = g.Key,
                    FeatureTypeName = types.FirstOrDefault(t => t.Id == g.Key)?.Name ?? "(unknown)",
                    MinPrice = g.Min(f => f.Price),
                    MaxPrice = g.Max(f => f.Price),
                    Count = g.Count()
                })
                .ToList();

            return grouped;
        }
        
        public async Task<List<FeatureDtos.FeatureResponseDtos>> GetByBuildingAndTypeAsync(string buildingId, string featureTypeId)
        {
            var features = await _repo.GetByBuildingIdAsync(buildingId);
            var filtered = features.Where(f => f.FeatureTypeId == featureTypeId).ToList();

            return filtered.Select(f => new FeatureDtos.FeatureResponseDtos
            {
                Id = f.Id,
                InvestmentId = f.InvestmentId,
                BuildingId = f.BuildingId,
                FeatureTypeId = f.FeatureTypeId,
                Price = f.Price,
                IsAvailable = f.IsAvailable,
                Description = f.Description
            }).ToList();
        }

        public async Task<List<FeatureDtos.FeatureResponseDtos>> GetByInvestmentAndTypeAsync(string investmentId, string featureTypeId)
        {
            var features = await _repo.GetByInvestmentIdAsync(investmentId);

            // tylko te z danego typu i bez przypisanego buildingId
            var filtered = features
                .Where(f => f.FeatureTypeId == featureTypeId && f.BuildingId == null)
                .ToList();

            return filtered.Select(f => new FeatureDtos.FeatureResponseDtos
            {
                Id = f.Id,
                InvestmentId = f.InvestmentId,
                BuildingId = f.BuildingId,
                FeatureTypeId = f.FeatureTypeId,
                Price = f.Price,
                IsAvailable = f.IsAvailable,
                Description = f.Description
            }).ToList();
        }
        
    }
}
