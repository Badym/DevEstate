using DevEstate.Api.Models;
using DevEstate.Api.Repositories;
using DevEstate.Dtos.DeveloperOpenData;
using DevEstate.Models;

namespace DevEstate.Api.Services
{
    public class DeveloperPriceService
    {
        private readonly DeveloperPriceRepository _repo;

        public DeveloperPriceService(DeveloperPriceRepository repo)
        {
            _repo = repo;
        }
        
        public async Task AddRecordAsync(DeveloperPriceRecordAggregatedDto dto)
        {
            if (dto.CenaZaM2 == null)
                throw new ArgumentException("CenaZaM2 cannot be null");

            var existing = await _repo.GetByFullLocationAsync(
                dto.Wojewodztwo!, dto.Powiat!, dto.Gmina!, dto.Miejscowosc!
            );

            if (existing != null)
                throw new Exception("Record already exists. Use update service instead.");

            int temp = 0;
            if (dto.Suma != 0 || dto.CenaZaM2.Value != 0)
                temp = dto.Ilosc;

            var entity = new DeveloperPriceEntity
            {
                Wojewodztwo = dto.Wojewodztwo,
                Powiat = dto.Powiat,
                Gmina = dto.Gmina,
                Miejscowosc = dto.Miejscowosc,

                CenaZaM2Total = dto.Suma ?? dto.CenaZaM2.Value,
                LiczbaMieszkan = temp,

                LastUpdated = DateTime.UtcNow
            };

            await _repo.CreateAsync(entity);
        }

        public async Task UpdateAsync(DeveloperPriceRecordUpdateDto dto)
        {
            if (dto.CenaZaM2 == null)
                throw new ArgumentException("CenaZaM2 cannot be null");

            var existing = await _repo.GetByRegionAsync(dto.Wojewodztwo!, dto.Powiat!);

            if (existing == null)
                throw new Exception("Record not found. Use ADD service instead.");

            existing.CenaZaM2Total += dto.CenaZaM2.Value;
            existing.LiczbaMieszkan++;
            existing.LastUpdated = DateTime.UtcNow;

            await _repo.UpdateAsync(existing);
        }
        
        public async Task AddOrUpdateAsync(DeveloperPriceRecordUpdateDto dto)
        {
            var normalizedWoj = Normalize(dto.Wojewodztwo);
            var normalizedPowiat = Normalize(dto.Powiat);

            if (dto.CenaZaM2 == null)
                throw new ArgumentException("CenaZaM2 cannot be null");

            // 🔥 FILTR POJEBANYCH CEN — centralnie tutaj
            if (dto.CenaZaM2 < 1500 || dto.CenaZaM2 > 25000)
            {
                Console.WriteLine(
                    $"[FILTER] Odrzucono cenę {dto.CenaZaM2} zł/m² ({normalizedWoj}/{normalizedPowiat})"
                );
                return; // pomijamy rekord
            }

            var existing = await _repo.GetByRegionAsync(normalizedWoj, normalizedPowiat);

            if (existing == null)
            {
                var entity = new DeveloperPriceEntity
                {
                    Wojewodztwo = normalizedWoj,
                    Powiat = normalizedPowiat,
                    CenaZaM2Total = dto.CenaZaM2.Value,
                    LiczbaMieszkan = 1,
                    LastUpdated = DateTime.UtcNow
                };

                await _repo.CreateAsync(entity);
                return;
            }

            existing.CenaZaM2Total += dto.CenaZaM2.Value;
            existing.LiczbaMieszkan++;
            existing.LastUpdated = DateTime.UtcNow;

            await _repo.UpdateAsync(existing);
        }


        
        private static string Normalize(string? text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return "";

            var normalized = text.Trim().ToLowerInvariant();

            normalized = normalized
                .Replace("ą", "a")
                .Replace("ć", "c")
                .Replace("ę", "e")
                .Replace("ł", "l")
                .Replace("ń", "n")
                .Replace("ó", "o")
                .Replace("ś", "s")
                .Replace("ź", "z")
                .Replace("ż", "z");

            return normalized;
        }
        
        public async Task<List<DeveloperPriceEntity>> GetFilteredAsync(string? woj, string? pow)
        {
            var all = await _repo.GetAllAsync();

            if (!string.IsNullOrWhiteSpace(woj))
                all = all.Where(x => 
                    x.Wojewodztwo.Equals(woj, StringComparison.OrdinalIgnoreCase)
                ).ToList();

            if (!string.IsNullOrWhiteSpace(pow))
                all = all.Where(x => 
                    x.Powiat.Equals(pow, StringComparison.OrdinalIgnoreCase)
                ).ToList();

            return all;
        }

        public async Task<List<string>> GetDistinctWojewodztwaAsync()
        {
            var all = await _repo.GetAllAsync();
            return all
                .Where(x => !string.IsNullOrWhiteSpace(x.Wojewodztwo))
                .Select(x => x.Wojewodztwo!)
                .Distinct()
                .OrderBy(x => x)
                .ToList();
        }
        
        public async Task<List<string>> GetDistinctPowiatyAsync(string? wojewodztwo)
        {
            var all = await _repo.GetAllAsync();

            if (!string.IsNullOrWhiteSpace(wojewodztwo))
                all = all.Where(x =>
                    x.Wojewodztwo.Equals(wojewodztwo, StringComparison.OrdinalIgnoreCase)
                ).ToList();

            return all
                .Where(x => !string.IsNullOrWhiteSpace(x.Powiat))
                .Select(x => x.Powiat!)
                .Distinct()
                .OrderBy(x => x)
                .ToList();
        }
        
        public async Task<object?> GetRegionAggregatedAsync(string woj, string? pow)
        {
            var normalizedWoj = Normalize(woj);
            var normalizedPow = Normalize(pow);

            // -------------------------
            // CASE 1: WOJ + POWIAT
            // -------------------------
            if (!string.IsNullOrWhiteSpace(pow))
            {
                var entry = await _repo.GetByRegionAsync(normalizedWoj, normalizedPow);
                if (entry == null) return null;

                return new
                {
                    wojewodztwo = entry.Wojewodztwo,
                    powiat = entry.Powiat,
                    avgPriceM2 = entry.CenaZaM2AVG,
                    count = entry.LiczbaMieszkan
                };
            }

            // -------------------------
            // CASE 2: TYLKO WOJEWÓDZTWO
            // -------------------------
            var list = await _repo.GetByWojewodztwoAsync(normalizedWoj);
            if (!list.Any()) return null;

            decimal total = list.Sum(x => x.CenaZaM2Total);
            int countSum = list.Sum(x => x.LiczbaMieszkan);

            return new
            {
                wojewodztwo = normalizedWoj,
                powiat = "(wszystkie)",
                avgPriceM2 = countSum > 0 ? Math.Round(total / countSum, 2) : 0,
                count = countSum
            };
        }

        


    }
}