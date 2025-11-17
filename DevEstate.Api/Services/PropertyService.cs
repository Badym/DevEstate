using DevEstate.Api.Dtos;
using DevEstate.Api.Models;
using DevEstate.Api.Repositories;
using DevEstate.Api.Services;

namespace DevEstate.Api.Services
{
    public class PropertyService
    {
        private readonly PropertyRepository _repo;
        private readonly PriceHistoryService _priceHistoryService;
        private readonly DocumentRepository _documentRepo;
        private readonly FeatureRepository _featureRepo;

        public PropertyService(PropertyRepository repo,DocumentRepository documentRepo,FeatureRepository featureRepo, PriceHistoryService priceHistoryService)
        {
            _repo = repo;
            _priceHistoryService =  priceHistoryService;
            _documentRepo = documentRepo;
            _featureRepo = featureRepo;
        }

        public async Task<PropertyDtos.PropertyResponseDtos> GetByIdAsync(string id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) throw new Exception("Property not found");

            return new PropertyDtos.PropertyResponseDtos
            {
                Id = entity.Id,
                ApartmentNumber = entity.ApartmentNumber,
                Type = entity.Type,
                Area = entity.Area,
                Price = entity.Price,
                PricePerMeter = entity.PricePerMeter,
                Status = entity.Status,
                InvestmentId = entity.InvestmentId,
                BuildingId = entity.BuildingId,
                Images = entity.Images,
                TotalPriceWithRequiredFeatures = entity.TotalPriceWithRequiredFeatures
            };
        }

        public async Task<List<PropertyDtos.PropertyResponseDtos>> GetAllAsync()
        {
            var entities = await _repo.GetAllAsync();
            return entities.Select(e => new PropertyDtos.PropertyResponseDtos
            {
                Id = e.Id,
                ApartmentNumber = e.ApartmentNumber,
                Type = e.Type,
                Area = e.Area,
                Price = e.Price,
                PricePerMeter = e.PricePerMeter,
                Status = e.Status,
                BuildingId = e.BuildingId,
                InvestmentId = e.InvestmentId,
                Images = e.Images,
                TotalPriceWithRequiredFeatures = e.TotalPriceWithRequiredFeatures
            }).ToList();
        }

        public async Task CreateAsync(PropertyDtos.PropertyCreateDtos dto)
        {
            decimal price = (decimal)dto.Price;  
            decimal area = (decimal)dto.Area;   

            decimal pricePerMeter = area > 0 ? Math.Round(price / area, 2) : 0;

            var entity = new Property
            {
                InvestmentId = dto.InvestmentId,
                BuildingId = dto.BuildingId,
                ApartmentNumber = dto.ApartmentNumber,
                Type = dto.Type,
                Area = dto.Area,
                TerraceArea = dto.TerraceArea,
                Price = dto.Price,
                PricePerMeter = pricePerMeter,
                Status = dto.Status
            };

            await _repo.CreateAsync(entity);

            await _priceHistoryService.CreateAsync(new PriceHistoryDtos.PriceHistoryCreateDtos
            {
                PropertyId = entity.Id!,
                Date = DateTime.UtcNow,
                NewPrice = entity.Price
            });

            await RecalculateTotalPriceAsync(entity);
        }
        
        public async Task UpdateAsync(string id, PropertyDtos.PropertyUpdateDtos dto)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) throw new Exception("Property not found");
            
            bool priceChanged = false;
            
            if (dto.Price.HasValue || dto.Status != null)
            {
                if (dto.Price.HasValue)
                {
                    priceChanged = dto.Price.Value != entity.Price;
                    
                    entity.Price = dto.Price.Value;

                    decimal price = (decimal)entity.Price; 
                    decimal area = (decimal)entity.Area;  

                    // Przeliczamy cena za m²
                    if (area > 0)
                    {
                        entity.PricePerMeter = Math.Round(price / area, 2);
                    }
                }
            }

            entity.Status = dto.Status ?? entity.Status;
            entity.Status = dto.Status ?? entity.Status;

            await _repo.UpdateAsync(entity);
            
            if (priceChanged)
            {
                await _priceHistoryService.CreateAsync(new PriceHistoryDtos.PriceHistoryCreateDtos
                {
                    PropertyId = entity.Id!,
                    Date = DateTime.UtcNow,
                    NewPrice = entity.Price
                });
            }
            
            await RecalculateTotalPriceAsync(entity);
        }

        public async Task DeleteAsync(string id)
        {
            // Usuń dokumenty powiązane z property
            await _documentRepo.DeleteByParentAsync("property", id);

            // Usuń property
            await _repo.DeleteAsync(id);
        }
        
        public async Task AddImageAsync(string propertyId, string fileUrl)
        {
            var property = await _repo.GetByIdAsync(propertyId);
            if (property != null)
            {
                property.Images ??= new List<string>();
                property.Images.Add(fileUrl);
                await _repo.UpdateAsync(property);
            }
        }
        
        public async Task<IEnumerable<PropertyDtos.PropertyResponseDtos>> GetByInvestmentIdAsync(string investmentId)
        {
            var properties = await _repo.GetByInvestmentIdAsync(investmentId);
            if (properties == null || !properties.Any())
                return Enumerable.Empty<PropertyDtos.PropertyResponseDtos>();

            return properties.Select(p => new PropertyDtos.PropertyResponseDtos
            {
                Id = p.Id!,
                ApartmentNumber = p.ApartmentNumber,
                Type = p.Type,
                Area = p.Area,
                Price = p.Price,
                PricePerMeter = p.PricePerMeter,
                Status = p.Status,
                BuildingId = p.BuildingId,
                InvestmentId = p.InvestmentId,
                Images = p.Images,
                TotalPriceWithRequiredFeatures = p.TotalPriceWithRequiredFeatures
            });
        }

        public async Task<IEnumerable<PropertyDtos.PropertyResponseDtos>> GetByInvestmentIdAndTypeAsync(string investmentId, string type)
        {
            // upewniamy się że typ jest małymi literami
            type = type.ToLower();

            var allProperties = await _repo.GetByInvestmentIdAsync(investmentId);
            if (allProperties == null || !allProperties.Any())
                return Enumerable.Empty<PropertyDtos.PropertyResponseDtos>();

            var filtered = allProperties.Where(p => p.Type.ToLower() == type).ToList();

            return filtered.Select(p => new PropertyDtos.PropertyResponseDtos
            {
                Id = p.Id!,
                ApartmentNumber = p.ApartmentNumber,
                Type = p.Type,
                Area = p.Area,
                Price = p.Price,
                PricePerMeter = p.PricePerMeter,
                Status = p.Status,
                InvestmentId = p.InvestmentId,
                Images = p.Images,
                TotalPriceWithRequiredFeatures = p.TotalPriceWithRequiredFeatures
            });
        }
        
        public Task<List<Property>> GetByBuildingIdAsync(string buildingId)
            => _repo.GetByBuildingIdAsync(buildingId);

        public Task<List<Property>> GetEntitiesByInvestmentIdAsync(string investmentId)
            => _repo.GetByInvestmentIdAsync(investmentId);
        
        public async Task RecalculateTotalPriceAsync(Property property)
        {
            decimal totalPrice = property.Price;

            var requiredFeatures = await _featureRepo.GetByInvestmentIdAsync(property.InvestmentId);

            if (!string.IsNullOrEmpty(property.BuildingId))
            {
                requiredFeatures = requiredFeatures.Where(f => f.BuildingId == property.BuildingId && f.IsRequired).ToList();
            }

            foreach (var feature in requiredFeatures)
            {
                if (feature.IsRequired) 
                {
                    totalPrice += feature.Price ?? 0; 
                }
            }

            property.TotalPriceWithRequiredFeatures = totalPrice;

            await _repo.UpdateAsync(property);
        }


    }
}
