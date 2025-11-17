using DevEstate.Api.Models;
using DevEstate.Api.Repositories;
using Microsoft.AspNetCore.Identity;

namespace DevEstate.Api.Services
{
    public class DataSeeder
    {
        private readonly CompanyRepository _companyRepo;
        private readonly InvestmentRepository _investmentRepo;
        private readonly BuildingRepository _buildingRepo;
        private readonly PropertyRepository _propertyRepo;
        private readonly UserRepository _userRepo;
        private readonly FeatureRepository _featureRepo;
        private readonly FeatureTypeRepository _featureTypeRepo;
        private readonly PriceHistoryRepository _priceHistoryRepo;
        private readonly PasswordHasher<User> _passwordHasher = new();

        public DataSeeder(
            CompanyRepository companyRepo,
            InvestmentRepository investmentRepo,
            BuildingRepository buildingRepo,
            PropertyRepository propertyRepo,
            UserRepository userRepo,
            FeatureRepository featureRepo,
            FeatureTypeRepository featureTypeRepo,
            PriceHistoryRepository priceHistoryRepo)
        {
            _companyRepo = companyRepo;
            _investmentRepo = investmentRepo;
            _buildingRepo = buildingRepo;
            _propertyRepo = propertyRepo;
            _userRepo = userRepo;
            _featureRepo = featureRepo;
            _featureTypeRepo = featureTypeRepo;
            _priceHistoryRepo = priceHistoryRepo;
        }

        public async Task SeedAsync()
        {
            if ((await _companyRepo.GetAllAsync()).Any()) return;

            // ========== FIRMA ==========
            var company = new Company
            {
                Name = "DevEstate Group",
                NIP = "1234567890",
                REGON = "987654321",
                KRS = "0000123456",
                Email = "kontakt@devestate.pl",
                Phone = "123-456-789",
                Website = "https://devestate.pl",
                Address = "ul. Przykładowa 1, 00-001 Warszawa",
                Description = "Nowoczesny deweloper realizujący inwestycje mieszkaniowe i domowe w całej Polsce.",
                CreatedAt = DateTime.UtcNow,
                LogoImage = "http://localhost:5086/uploads/images/logoO.jpg"
            };
            await _companyRepo.CreateAsync(company);

            // ========== FEATURE TYPES ==========
            var featureTypes = new List<FeatureType>
            {
                new() { Name = "Winda", UnitName = null },
                new() { Name = "Miejsce parkingowe", UnitName = "szt." },
                new() { Name = "Komórka lokatorska", UnitName = "szt." },
                new() { Name = "Monitoring", UnitName = null },
                new() { Name = "Plac zabaw", UnitName = null },
                new() { Name = "Ogródek", UnitName = "m²" },
                new() { Name = "Garaż podziemny", UnitName = "szt." }
            };
            foreach (var ft in featureTypes)
                await _featureTypeRepo.CreateAsync(ft);

            string GetTypeId(string name) =>
                featureTypes.First(f => f.Name == name).Id!;

            // ========== INWESTYCJA 1: Libelta Residence ==========
            var libelta = new Investment
            {
                CompanyId = company.Id,
                Name = "Libelta Residence",
                City = "Poznań",
                Street = "ul. Libelta 15",
                PostalCode = "61-706",
                Description = "Ekskluzywne apartamenty w centrum Poznania — połączenie historii i nowoczesności.",
                Status = "Aktualne",
                Images = new List<string>
                {
                    "http://localhost:5086/uploads/images/Investment_68fbb4167b6000293487edf9_3624ff3819944cc68f0ffd275892a398.webp"
                },
                CreatedAt = DateTime.UtcNow
            };
            await _investmentRepo.CreateAsync(libelta);

            // --- Budynki ---
            var libeltaBuildingA = new Building
            {
                InvestmentId = libelta.Id,
                BuildingNumber = "A",
                Description = "6-piętrowy budynek z garażem podziemnym i windą.",
                Status = "Aktualne",
                Images = new List<string>
                {
                    "http://localhost:5086/uploads/images/Building_68fbb5bd7038d7a814fda4ea_f4910ea3e97545f8b541c874df428c1b.webp"
                },
                CreatedAt = DateTime.UtcNow
            };
            await _buildingRepo.CreateAsync(libeltaBuildingA);

            var libeltaBuildingB = new Building
            {
                InvestmentId = libelta.Id,
                BuildingNumber = "B",
                Description = "Nowoczesny budynek z widokiem na park i apartamentami premium.",
                Status = "Aktualne",
                Images = new List<string>
                {
                    "http://localhost:5086/uploads/images/Building_68fbb5bd7038d7a814fda4ea_f4910ea3e97545f8b541c874df428c1b.webp"
                },
                CreatedAt = DateTime.UtcNow
            };
            await _buildingRepo.CreateAsync(libeltaBuildingB);

            // --- Mieszkania ---
            var libeltaApts = new List<Property>
            {
                new() { InvestmentId = libelta.Id, BuildingId = libeltaBuildingA.Id, ApartmentNumber = "1A", Type = "apartment", Area = 48.5, Price = 510000, PricePerMeter = 10515, Status = "Aktualne", Images = new() { "/images/apartment1.jpg" } },
                new() { InvestmentId = libelta.Id, BuildingId = libeltaBuildingA.Id, ApartmentNumber = "2B", Type = "apartment", Area = 57.2, Price = 585000, PricePerMeter = 10227, Status = "Zarezerwowane", Images = new() { "/images/apartment2.jpg" } },
                new() { InvestmentId = libelta.Id, BuildingId = libeltaBuildingB.Id, ApartmentNumber = "3C", Type = "apartment", Area = 73.1, Price = 720000, PricePerMeter = 9850, Status = "Aktualne", Images = new() { "/images/apartment3.jpg" } }
            };
            foreach (var apt in libeltaApts)
                await _propertyRepo.CreateAsync(apt);

            // --- Feature'y ---
            await _featureRepo.CreateAsync(new Feature
            {
                InvestmentId = libelta.Id,
                BuildingId = libeltaBuildingA.Id,
                FeatureTypeId = GetTypeId("Winda"),
                Description = "Cicha, energooszczędna winda obsługująca wszystkie piętra",
                IsRequired = false
            });
            await _featureRepo.CreateAsync(new Feature
            {
                InvestmentId = libelta.Id,
                BuildingId = libeltaBuildingA.Id,
                FeatureTypeId = GetTypeId("Garaż podziemny"),
                Price = 45000,
                Description = "Obowiązkowy garaż podziemny dla każdego lokalu",
                IsRequired = true
            });
            await _featureRepo.CreateAsync(new Feature
            {
                InvestmentId = libelta.Id,
                BuildingId = libeltaBuildingB.Id,
                FeatureTypeId = GetTypeId("Monitoring"),
                Description = "Całodobowy monitoring i ochrona budynku",
                IsRequired = false
            });

            // ========== INWESTYCJA 2: Jasne Tarasy ==========
            var jasne = new Investment
            {
                CompanyId = company.Id,
                Name = "Jasne Tarasy",
                City = "Gdańsk",
                Street = "ul. Nadmorska 12",
                PostalCode = "80-341",
                Description = "Nowoczesne osiedle nad morzem z widokiem na Zatokę Gdańską.",
                Status = "Aktualne",
                Images = new List<string>
                {
                    "http://localhost:5086/uploads/images/Investment_68f4c723543280f07a4c6352_17a24ea03fb04f58a680688e49c205f0.jpg"
                },
                CreatedAt = DateTime.UtcNow
            };
            await _investmentRepo.CreateAsync(jasne);

            var jasneBuildingA = new Building
            {
                InvestmentId = jasne.Id,
                BuildingNumber = "A",
                Description = "Dwupiętrowy budynek z widokiem na morze.",
                Status = "Aktualne",
                Images = new List<string>
                {
                    "http://localhost:5086/uploads/images/Building_68fbb5bd7038d7a814fda4ea_f4910ea3e97545f8b541c874df428c1b.webp"
                },
                CreatedAt = DateTime.UtcNow
            };
            await _buildingRepo.CreateAsync(jasneBuildingA);

            var jasneHouses = new List<Property>
            {
                new() { InvestmentId = jasne.Id, ApartmentNumber = "33E", Type = "house", Area = 120, Price = 920000, PricePerMeter = 7666, Status = "Aktualne", Images = new() { "/images/house1.jpg" } },
                new() { InvestmentId = jasne.Id, ApartmentNumber = "33F", Type = "house", Area = 125, Price = 950000, PricePerMeter = 7600, Status = "Sprzedane", Images = new() { "/images/house2.jpg" } },
                new() { InvestmentId = jasne.Id, ApartmentNumber = "33G", Type = "house", Area = 135, Price = 990000, PricePerMeter = 7333, Status = "Aktualne", Images = new() { "/images/house3.jpg" } }
            };
            foreach (var house in jasneHouses)
                await _propertyRepo.CreateAsync(house);

            await _featureRepo.CreateAsync(new Feature
            {
                InvestmentId = jasne.Id,
                BuildingId = jasneBuildingA.Id,
                FeatureTypeId = GetTypeId("Plac zabaw"),
                Description = "Bezpieczny teren dla dzieci z ogrodzeniem",
                IsRequired = true,
                Price = 10000
            });

            // --- Historia cen ---
            var house33E = jasneHouses.First(h => h.ApartmentNumber == "33E");
            await _priceHistoryRepo.CreateAsync(new PriceHistory
            {
                PropertyId = house33E.Id,
                Date = DateTime.UtcNow.AddMonths(-3),
                NewPrice = 880000
            });
            await _priceHistoryRepo.CreateAsync(new PriceHistory
            {
                PropertyId = house33E.Id,
                Date = DateTime.UtcNow.AddMonths(-1),
                NewPrice = 910000
            });

            // ========== INWESTYCJA 3: Zielona Polana ==========
            var zielonaPolana = new Investment
            {
                CompanyId = company.Id,
                Name = "Zielona Polana",
                City = "Kraków",
                Street = "ul. Zielona 5",
                PostalCode = "30-100",
                Description = "Nowoczesne domy w spokojnej, zielonej okolicy na obrzeżach Krakowa.",
                Status = "Sprzedane",
                Images = new List<string>
                {
                    "http://localhost:5086/uploads/images/Investment_68fba5e4d34c0daf6c4272c7_fcbd42ac2bca4a05a953c997ce864af9.jpg"
                },
                CreatedAt = DateTime.UtcNow
            };
            await _investmentRepo.CreateAsync(zielonaPolana);

            var zielonaBuildingA = new Building
            {
                InvestmentId = zielonaPolana.Id,
                BuildingNumber = "A",
                Description = "Osiedle domów jednorodzinnych w zabudowie bliźniaczej.",
                Status = "Sprzedane",
                Images = new List<string>
                {
                    "http://localhost:5086/uploads/images/Building_68fbb5bd7038d7a814fda4ea_f4910ea3e97545f8b541c874df428c1b.webp"
                },
                CreatedAt = DateTime.UtcNow
            };
            await _buildingRepo.CreateAsync(zielonaBuildingA);

            var zielonaPolanaHouses = new List<Property>
            {
                new() { InvestmentId = zielonaPolana.Id, BuildingId = zielonaBuildingA.Id, ApartmentNumber = "1A", Type = "house", Area = 120, Price = 950000, PricePerMeter = 7916, Status = "Sprzedane", Images = new() { "/images/house1.jpg" } },
                new() { InvestmentId = zielonaPolana.Id, BuildingId = zielonaBuildingA.Id, ApartmentNumber = "2B", Type = "house", Area = 135, Price = 1050000, PricePerMeter = 7777, Status = "Sprzedane", Images = new() { "/images/house2.jpg" } }
            };
            foreach (var house in zielonaPolanaHouses)
                await _propertyRepo.CreateAsync(house);

            await _featureRepo.CreateAsync(new Feature
            {
                InvestmentId = zielonaPolana.Id,
                BuildingId = zielonaBuildingA.Id,
                FeatureTypeId = GetTypeId("Ogródek"),
                Description = "Każdy dom posiada własny ogród (obowiązkowy dodatek)",
                IsRequired = true,
                Price = 20000
            });

            // ========== ADMIN ==========
            var admin = new User
            {
                Email = "admin@devestate.pl",
                FullName = "Administrator",
                Role = "Admin",
                CreatedAt = DateTime.UtcNow,
                PasswordHash = _passwordHasher.HashPassword(null!, "admin123")
            };
            await _userRepo.CreateAsync(admin);
        }
    }
}
