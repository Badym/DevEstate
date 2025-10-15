using DevEstate.Api.Dtos;
using DevEstate.Api.Models;
using DevEstate.Api.Repositories;

namespace DevEstate.Api.Services
{
    public class InvestmentService
    {
        private readonly InvestmentRepository _repo;

        public InvestmentService(InvestmentRepository repo)
        {
            _repo = repo;
        }

        public async Task<InvestmentDtos.Response> GetByIdAsync(string id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) throw new Exception("Investment not found");

            return new InvestmentDtos.Response
            {
                Id = entity.Id,
                Name = entity.Name,
                City = entity.City,
                Street = entity.Street,
                PostalCode = entity.PostalCode,
                Description = entity.Description,
                Status = entity.Status,
                CreatedAt = entity.CreatedAt
            };
        }

        public async Task<List<InvestmentDtos.Response>> GetAllAsync()
        {
            var entities = await _repo.GetAllAsync();
            return entities.Select(e => new InvestmentDtos.Response
            {
                Id = e.Id,
                Name = e.Name,
                City = e.City,
                Street = e.Street,
                PostalCode = e.PostalCode,
                Description = e.Description,
                Status = e.Status,
                CreatedAt = e.CreatedAt
            }).ToList();
        }

        public async Task CreateAsync(InvestmentDtos.Create dto)
        {
            var entity = new Investment
            {
                CompanyId = dto.CompanyId,
                Name = dto.Name,
                City = dto.City,
                Street = dto.Street,
                PostalCode = dto.PostalCode,
                Description = dto.Description,
                Status = dto.Status
            };
            await _repo.CreateAsync(entity);
        }

        public async Task UpdateAsync(string id, InvestmentDtos.Update dto)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) throw new Exception("Investment not found");

            entity.Name = dto.Name ?? entity.Name;
            entity.City = dto.City ?? entity.City;
            entity.Street = dto.Street ?? entity.Street;
            entity.PostalCode = dto.PostalCode ?? entity.PostalCode;
            entity.Description = dto.Description ?? entity.Description;
            entity.Status = dto.Status ?? entity.Status;

            await _repo.UpdateAsync(entity);
        }

        public async Task DeleteAsync(string id)
        {
            await _repo.DeleteAsync(id);
        }
        
        public async Task AddImageAsync(string investmentId, string fileUrl)
        {
            var investment = await _repo.GetByIdAsync(investmentId);
            if (investment != null)
            {
                investment.Images ??= new List<string>();
                investment.Images.Add(fileUrl);
                await _repo.UpdateAsync(investment);
            }
        }

    }
}
