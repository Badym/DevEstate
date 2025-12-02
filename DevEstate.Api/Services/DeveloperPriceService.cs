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
            if (dto.CenaZaM2 < 4000 || dto.CenaZaM2 > 25000)
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

    }
}