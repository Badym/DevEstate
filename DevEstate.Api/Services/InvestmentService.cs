using DevEstate.Api.Dtos;
using DevEstate.Api.Models;
using DevEstate.Api.Repositories;

namespace DevEstate.Api.Services
{
    public class InvestmentService
    {
        private readonly InvestmentRepository _investmentRepo;
        private readonly DocumentRepository _documentRepo;
        private readonly BuildingService _buildingService;
        private readonly PropertyService _propertyService;

        public InvestmentService(
            InvestmentRepository investmentRepo,
            DocumentRepository documentRepo,
            BuildingService buildingService,
            PropertyService propertyService)
        {
            _investmentRepo = investmentRepo;
            _documentRepo = documentRepo;
            _buildingService = buildingService;
            _propertyService = propertyService;
        }

        public async Task<InvestmentDtos.InvestmentResponseDtos> GetByIdAsync(string id)
        {
            var entity = await _investmentRepo.GetByIdAsync(id);
            if (entity == null) throw new Exception("Investment not found");

            return new InvestmentDtos.InvestmentResponseDtos
            {
                Id = entity.Id,
                Name = entity.Name,
                City = entity.City,
                Street = entity.Street,
                PostalCode = entity.PostalCode,
                Description = entity.Description,
                Status = entity.Status,
                CreatedAt = entity.CreatedAt,
                Images = entity.Images,
                InvestmentMunicipality =  entity.InvestmentMunicipality,
                InvestmentProvince = entity.InvestmentProvince,
                InvestmentCounty = entity.InvestmentCounty,
            };
        }

        public async Task<List<InvestmentDtos.InvestmentResponseDtos>> GetAllAsync()
        {
            var entities = await _investmentRepo.GetAllAsync();
            return entities.Select(e => new InvestmentDtos.InvestmentResponseDtos()
            {
                Id = e.Id,
                Name = e.Name,
                City = e.City,
                Street = e.Street,
                PostalCode = e.PostalCode,
                Description = e.Description,
                Status = e.Status,
                CreatedAt = e.CreatedAt,
                Images = e.Images,
                InvestmentMunicipality =  e.InvestmentMunicipality,
                InvestmentProvince = e.InvestmentProvince,
                InvestmentCounty = e.InvestmentCounty,
            }).ToList();
        }

        public async Task CreateAsync(InvestmentDtos.InvestmentCreateDtos dto)
        {
            var entity = new Investment
            {
                CompanyId = dto.CompanyId,
                
                Name = dto.Name,
                City = dto.City,
                Street = dto.Street,
                PostalCode = dto.PostalCode,
                Description = dto.Description,
                Status = dto.Status,
                InvestmentMunicipality =  dto.InvestmentMunicipality,
                InvestmentProvince = dto.InvestmentProvince,
                InvestmentCounty = dto.InvestmentCounty,
            };
            await _investmentRepo.CreateAsync(entity);
        }

        public async Task UpdateAsync(string id, InvestmentDtos.InvestmentUpdateDtos dto)
        {
            var entity = await _investmentRepo.GetByIdAsync(id);
            if (entity == null) throw new Exception("Investment not found");

            entity.Name = dto.Name ?? entity.Name;
            entity.City = dto.City ?? entity.City;
            entity.Street = dto.Street ?? entity.Street;
            entity.PostalCode = dto.PostalCode ?? entity.PostalCode;
            entity.Description = dto.Description ?? entity.Description;
            entity.Status = dto.Status ?? entity.Status;
            entity.InvestmentProvince = dto.InvestmentProvince ?? entity.InvestmentProvince;
            entity.InvestmentCounty = dto.InvestmentCounty ?? entity.InvestmentCounty;
            entity.InvestmentMunicipality = dto.InvestmentMunicipality ?? entity.InvestmentMunicipality;

            await _investmentRepo.UpdateAsync(entity);
        }

        public async Task DeleteAsync(string id)
        {
            // Usuń dokumenty inwestycji
            await _documentRepo.DeleteByParentAsync("investment", id);

            // Usuń budynki przypisane do inwestycji
            var buildings = await _buildingService.GetEntitiesByInvestmentIdAsync(id);
            if (buildings != null && buildings.Any())
            {
                foreach (var building in buildings)
                    await _buildingService.DeleteAsync(building.Id);
            }

            // Usuń domy niezależne (properties bez buildingId)
            var properties = await _propertyService.GetEntitiesByInvestmentIdAsync(id);
            if (properties != null && properties.Any())
            {
                foreach (var property in properties)
                {
                    if (string.IsNullOrEmpty(property.BuildingId))
                        await _propertyService.DeleteAsync(property.Id);
                }
            }

            // Usuń inwestycję
            await _investmentRepo.DeleteAsync(id);
        }
        
        public async Task AddImageAsync(string investmentId, string fileUrl)
        {
            var investment = await _investmentRepo.GetByIdAsync(investmentId);
            if (investment != null)
            {
                investment.Images ??= new List<string>();
                investment.Images.Add(fileUrl);
                await _investmentRepo.UpdateAsync(investment);
            }
        }
        
        public async Task<List<InvestmentDtos.InvestmentResponseDtos>> GetByStatusAsync(string status)
        {
            var entities = await _investmentRepo.GetAllAsync();

            // Filtrowanie inwestycji na podstawie statusu
            var filteredInvestments = entities
                .Where(e => e.Status.Equals(status, StringComparison.OrdinalIgnoreCase))
                .Select(e => new InvestmentDtos.InvestmentResponseDtos
                {
                    Id = e.Id,
                    Name = e.Name,
                    City = e.City,
                    Street = e.Street,
                    PostalCode = e.PostalCode,
                    Description = e.Description,
                    Status = e.Status,
                    CreatedAt = e.CreatedAt,
                    Images = e.Images,
                    InvestmentMunicipality =  e.InvestmentMunicipality,
                    InvestmentProvince = e.InvestmentProvince,
                    InvestmentCounty = e.InvestmentCounty,
                })
                .ToList();

            return filteredInvestments;
        }

        public async Task<InvestmentDtos.InvestmentResponseDtos?> GetByNameAsync(string name)
        {
            var entity = (await _investmentRepo.GetAllAsync())
                .FirstOrDefault(i => i.Name.ToLower() == name.ToLower());

            if (entity == null)
                return null;

            return new InvestmentDtos.InvestmentResponseDtos
            {
                Id = entity.Id,
                Name = entity.Name,
                City = entity.City,
                Street = entity.Street,
                PostalCode = entity.PostalCode,
                Description = entity.Description,
                Status = entity.Status,
                CreatedAt = entity.CreatedAt,
                Images = entity.Images,
                InvestmentMunicipality =  entity.InvestmentMunicipality,
                InvestmentProvince = entity.InvestmentProvince,
                InvestmentCounty = entity.InvestmentCounty,
            };
        }


    }
}
