using DevEstate.Api.Models;
using DevEstate.Api.Repositories;

namespace DevEstate.Api.Services
{
    public class CsvDateService
    {
        private readonly CsvDateRepository _repo;

        public CsvDateService(CsvDateRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<CsvDate>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<CsvDate?> GetLastAsync()
        {
            return await _repo.GetLastAsync();
        }

        public async Task<CsvDate?> GetByDateAsync(DateTime date)
        {
            return await _repo.GetByDateAsync(date);
        }

        public async Task AddAsync(DateTime date, string fileName)
        {
            var existing = await _repo.GetByDateAsync(date);
            if (existing != null)
                throw new Exception("Entry for this date already exists");

            var entity = new CsvDate
            {
                Date = date,
                FileName = fileName
            };

            await _repo.CreateAsync(entity);
        }

        public async Task DeleteAsync(string id)
        {
            await _repo.DeleteAsync(id);
        }

        public async Task DeleteByDateAsync(DateTime date)
        {
            await _repo.DeleteByDateAsync(date);
        }
    }
}