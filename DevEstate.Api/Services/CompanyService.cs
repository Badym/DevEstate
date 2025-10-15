using DevEstate.Api.Dtos;
using DevEstate.Api.Models;
using DevEstate.Api.Repositories;

namespace DevEstate.Api.Services
{
    public class CompanyService
    {
        private readonly CompanyRepository _repo;

        public CompanyService(CompanyRepository repo)
        {
            _repo = repo;
        }

        public async Task<CompanyDtos.Response> GetByIdAsync(string id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) throw new Exception("Company not found");

            return new CompanyDtos.Response
            {
                Id = entity.Id,
                Name = entity.Name,
                Email = entity.Email,
                Phone = entity.Phone,
                Website = entity.Website,
                Address = entity.Address,
                Description = entity.Description,
                CreatedAt = entity.CreatedAt
            };
        }

        public async Task<List<CompanyDtos.Response>> GetAllAsync()
        {
            var entities = await _repo.GetAllAsync();
            return entities.Select(e => new CompanyDtos.Response
            {
                Id = e.Id,
                Name = e.Name,
                Email = e.Email,
                Phone = e.Phone,
                Website = e.Website,
                Address = e.Address,
                Description = e.Description,
                CreatedAt = e.CreatedAt
            }).ToList();
        }

        public async Task CreateAsync(CompanyDtos.Create dto)
        {
            var entity = new Company
            {
                Name = dto.Name,
                NIP = dto.NIP,
                REGON = dto.REGON,
                KRS = dto.KRS,
                Email = dto.Email,
                Phone = dto.Phone,
                Website = dto.Website,
                Address = dto.Address,
                Description = dto.Description,
                CreatedAt = DateTime.UtcNow
            };
            await _repo.CreateAsync(entity);
        }

        public async Task UpdateAsync(string id, CompanyDtos.Update dto)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) throw new Exception("Company not found");

            entity.Name = dto.Name ?? entity.Name;
            entity.Email = dto.Email ?? entity.Email;
            entity.Phone = dto.Phone ?? entity.Phone;
            entity.Website = dto.Website ?? entity.Website;
            entity.Address = dto.Address ?? entity.Address;
            entity.Description = dto.Description ?? entity.Description;

            await _repo.UpdateAsync(entity);
        }

        public async Task DeleteAsync(string id)
        {
            await _repo.DeleteAsync(id);
        }
    }
}
