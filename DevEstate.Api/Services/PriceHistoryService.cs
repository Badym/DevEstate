using DevEstate.Api.Dtos;
using DevEstate.Api.Models;
using DevEstate.Api.Repositories;

namespace DevEstate.Api.Services
{
    public class PriceHistoryService
    {
        private readonly PriceHistoryRepository _repo;

        public PriceHistoryService(PriceHistoryRepository repo)
        {
            _repo = repo;
        }

        public async Task<PriceHistoryDtos.PriceHistoryResponseDtos> GetByIdAsync(string id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) throw new Exception("PriceHistory not found");

            return new PriceHistoryDtos.PriceHistoryResponseDtos
            {
                Id = entity.Id,
                PropertyId = entity.PropertyId,
                Date = entity.Date,
                NewPrice = entity.NewPrice
            };
        }

        public async Task<List<PriceHistoryDtos.PriceHistoryResponseDtos>> GetAllAsync()
        {
            var entities = await _repo.GetAllAsync();
            return entities.Select(e => new PriceHistoryDtos.PriceHistoryResponseDtos
            {
                Id = e.Id,
                PropertyId = e.PropertyId,
                Date = e.Date,
                NewPrice = e.NewPrice
            }).ToList();
        }

        public async Task CreateAsync(PriceHistoryDtos.PriceHistoryCreateDtos dto)
        {
            var entity = new PriceHistory
            {
                PropertyId = dto.PropertyId,
                Date = dto.Date,
                NewPrice = dto.NewPrice
            };
            await _repo.CreateAsync(entity);
        }

        public async Task DeleteAsync(string id)
        {
            await _repo.DeleteAsync(id);
        }
        
        public async Task<List<PriceHistoryDtos.PriceHistoryResponseDtos>> GetByPropertyIdAsync(string propertyId)
        {
            var entities = await _repo.GetByPropertyIdAsync(propertyId);
            if (entities == null || !entities.Any())
                return new List<PriceHistoryDtos.PriceHistoryResponseDtos>();

            return entities
                .OrderByDescending(e => e.Date)
                .Select(e => new PriceHistoryDtos.PriceHistoryResponseDtos
                {
                    Id = e.Id,
                    PropertyId = e.PropertyId,
                    Date = e.Date,
                    NewPrice = e.NewPrice
                })
                .ToList();
        }
    }
}