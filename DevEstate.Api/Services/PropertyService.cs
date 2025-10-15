using DevEstate.Api.Dtos;
using DevEstate.Api.Models;
using DevEstate.Api.Repositories;

namespace DevEstate.Api.Services
{
    public class PropertyService
    {
        private readonly PropertyRepository _repo;

        public PropertyService(PropertyRepository repo)
        {
            _repo = repo;
        }

        public async Task<PropertyDtos.Response> GetByIdAsync(string id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) throw new Exception("Property not found");

            return new PropertyDtos.Response
            {
                Id = entity.Id,
                ApartmentNumber = entity.ApartmentNumber,
                Type = entity.Type,
                Area = entity.Area,
                Price = entity.Price,
                PricePerMeter = entity.PricePerMeter,
                Status = entity.Status,
                BuildingNumber = entity.BuildingId,
                InvestmentName = entity.InvestmentId
            };
        }

        public async Task<List<PropertyDtos.Response>> GetAllAsync()
        {
            var entities = await _repo.GetAllAsync();
            return entities.Select(e => new PropertyDtos.Response
            {
                Id = e.Id,
                ApartmentNumber = e.ApartmentNumber,
                Type = e.Type,
                Area = e.Area,
                Price = e.Price,
                PricePerMeter = e.PricePerMeter,
                Status = e.Status,
                BuildingNumber = e.BuildingId,
                InvestmentName = e.InvestmentId
            }).ToList();
        }

        public async Task CreateAsync(PropertyDtos.Create dto)
        {
            var entity = new Property
            {
                InvestmentId = dto.InvestmentId,
                BuildingId = dto.BuildingId,
                ApartmentNumber = dto.ApartmentNumber,
                Type = dto.Type,
                Area = dto.Area,
                TerraceArea = dto.TerraceArea,
                Price = dto.Price,
                PricePerMeter = dto.PricePerMeter,
                Status = dto.Status
            };
            await _repo.CreateAsync(entity);
        }

        public async Task UpdateAsync(string id, PropertyDtos.Update dto)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) throw new Exception("Property not found");

            entity.Price = dto.Price ?? entity.Price;
            entity.Status = dto.Status ?? entity.Status;

            await _repo.UpdateAsync(entity);
        }

        public async Task DeleteAsync(string id)
        {
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

    }
}
