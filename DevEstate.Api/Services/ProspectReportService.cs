using DevEstate.Api.Dtos;
using DevEstate.Api.Repositories;
using DevEstate.Api.Models;

namespace DevEstate.Api.Services
{
    public class ProspectReportService
    {
        private readonly CompanyRepository _companyRepo;
        private readonly InvestmentRepository _investmentRepo;
        private readonly PropertyRepository _propertyRepo;
        private readonly FeatureRepository _featureRepo;
        private readonly PriceHistoryRepository _priceHistoryRepo;
        private readonly BuildingService _buildingService;
        
        private readonly IWebHostEnvironment _env;


        public ProspectReportService(
            CompanyRepository companyRepo,
            InvestmentRepository investmentRepo,
            PropertyRepository propertyRepo,
            FeatureRepository featureRepo,
            PriceHistoryRepository priceHistoryRepo,
            BuildingService buildingService,
            IWebHostEnvironment env)
        {
            _companyRepo = companyRepo;
            _investmentRepo = investmentRepo;
            _propertyRepo = propertyRepo;
            _featureRepo = featureRepo;
            _priceHistoryRepo = priceHistoryRepo;
            _buildingService = buildingService;
            _env = env;
        }

public async Task<List<ProspectReportDtos.Row>> GenerateReportAsync()
{
    var company = (await _companyRepo.GetAllAsync()).FirstOrDefault();
    if (company == null) throw new Exception("Company data not found");

    var investments = await _investmentRepo.GetAllAsync();
    var properties = await _propertyRepo.GetAllAsync();
    var features = await _featureRepo.GetAllAsync();
    var priceHistories = await _priceHistoryRepo.GetAllAsync();

    var rows = new List<ProspectReportDtos.Row>();

    // 🔹 2. Przejście po wszystkich nieruchomościach (tylko dostępne)
    foreach (var property in properties.Where(p => p.Status == "Aktualne"))
    {
        var investment = investments.FirstOrDefault(i => i.Id == property.InvestmentId);
        if (investment == null)
            continue;

        // 🔹 3. Ostatnia znana cena z historii
        var latestPrice = priceHistories
            .Where(ph => ph.PropertyId == property.Id)
            .OrderByDescending(ph => ph.Date)
            .FirstOrDefault();

        // 🔹 4. Dodatki powiązane z inwestycją (np. garaże, komórki), uwzględniając tylko wymagane
        var propertyFeatures = features
            .Where(f => f.InvestmentId == investment.Id && f.IsRequired)
            .ToList();

        // 🔹 5. Pobierz numer budynku powiązanego z nieruchomością (jeśli jest przypisany do budynku)
        string? buildingNumber = null;

        if (!string.IsNullOrEmpty(property.BuildingId))
        {
            var building = await _buildingService.GetByIdAsync(property.BuildingId);  // Pobierz budynek powiązany z property
            buildingNumber = building?.BuildingNumber;  // Pobierz numer budynku (jeśli jest)
        }

        // 🔹 6. Wygeneruj dynamicznie adres prospektu (bez pola w bazie!)
        string prospectUrl = GenerateProspectUrl(company.Website, investment.Name);

        // 🔹 7. Złóż wiersz raportu
        rows.Add(new ProspectReportDtos.Row
        {
            DeveloperName = company.Name,
            LegalForm = company.LegalForm,
            KRS = company.KRS,
            CEIDGNumber = company.CEIDGNumber,
            NIP = company.NIP,
            REGON = company.REGON,
            Phone = company.Phone,
            Fax = company.Fax,
            Email = company.Email,
            Website = company.Website,
            HeadquartersProvince = company.Province,
            HeadquartersCounty = company.County,
            HeadquartersMunicipality = company.Municipality,
            HeadquartersCity = company.City,
            HeadquartersStreet = company.Street,
            HeadquartersBuildingNumber = company.BuildingNumber,
            HeadquartersApartmentNumber = company.ApartmentNumber,
            HeadquartersPostalCode = company.PostalCode,
            ContactMethod = company.ContactMethod,
            InvestmentName = investment.Name,
            InvestmentCity = investment.City,
            InvestmentStreet = investment.Street,
            InvestmentBuildingNumber = buildingNumber,  // Dodajemy numer budynku lub null
            InvestmentPostalCode = investment.PostalCode,

            PropertyType = property.Type,
            ApartmentNumber = property.ApartmentNumber,
            Area = property.Area,
            PricePerM2 = property.PricePerMeter,
            TotalPrice = property.Price,
            FullPrice = property.Price, // w przyszłości: dodamy sumowanie z dodatkami
            PriceFromDate = latestPrice?.Date,

            AttachedParts = string.Join(", ", propertyFeatures.Select(f => f.FeatureTypeId)),
            AttachedPartsLabels = string.Join(", ", propertyFeatures.Select(f => f.Description)),
            AttachedPartsPrice = propertyFeatures.Sum(f => f.Price ?? 0),
            AttachedPartsPriceFromDate = latestPrice?.Date,

            ProspectUrl = prospectUrl,

            InvestmentProvince = investment.InvestmentProvince,
            InvestmentCounty = investment.InvestmentCounty,
            InvestmentMunicipality = investment.InvestmentMunicipality,
        });
    }

    return rows;
}

        public async Task<string> GenerateCsvReportAsync()
{
    // 1. Pobierz dane z istniejącego raportu
    var rows = await GenerateReportAsync(); // Zmieniamy: używamy istniejącej metody do pobrania danych

    var csvRows = new List<string>();

    // 2. Definicja nagłówków CSV (kolejność powinna być zgodna z danymi w DTO)
    var headers = new List<string>
    {
        "Nazwa dewelopera", "Forma prawna dewelopera", "Nr KRS", "Nr wpisu do CEiDG", "Nr NIP", "Nr REGON", 
        "Nr telefonu", "Adres poczty elektronicznej", "Nr faxu", "Adres strony internetowej dewelopera", 
        "Województwo adresu siedziby/głównego miejsca wykonywania działalności gospodarczej dewelopera", 
        "Powiat adresu siedziby/głównego miejsca wykonywania działalności gospodarczej dewelopera", 
        "Gmina adresu siedziby/głównego miejsca wykonywania działalności gospodarczej dewelopera", 
        "Miejscowość adresu siedziby/głównego miejsca wykonywania działalności gospodarczej dewelopera", 
        "Ulica adresu siedziby/głównego miejsca wykonywania działalności gospodarczej dewelopera", 
        "Nr nieruchomości adresu siedziby/głównego miejsca wykonywania działalności gospodarczej dewelopera", 
        "Nr lokalu adresu siedziby/głównego miejsca wykonywania działalności gospodarczej dewelopera", 
        "Kod pocztowy adresu siedziby/głównego miejsca wykonywania działalności gospodarczej dewelopera", 
        "Województwo adresu lokalu, w którym prowadzona jest sprzedaż", 
        "Powiat adresu lokalu, w którym prowadzona jest sprzedaż", 
        "Gmina adresu lokalu, w którym prowadzona jest sprzedaż", 
        "Miejscowość adresu lokalu, w którym prowadzona jest sprzedaż", 
        "Ulica adresu lokalu, w którym prowadzona jest sprzedaż", 
        "Nr nieruchomości adresu lokalu, w którym prowadzona jest sprzedaż", 
        "Nr lokalu adresu lokalu, w którym prowadzona jest sprzedaż", 
        "Kod pocztowy adresu lokalu, w którym prowadzona jest sprzedaż", 
        "Dodatkowe lokalizacje, w których prowadzona jest sprzedaż", 
        "Sposób kontaktu nabywcy z deweloperem", 
        "Województwo lokalizacji przedsięwzięcia deweloperskiego lub zadania inwestycyjnego", 
        "Powiat lokalizacji przedsięwzięcia deweloperskiego lub zadania inwestycyjnego", 
        "Gmina lokalizacji przedsięwzięcia deweloperskiego lub zadania inwestycyjnego", 
        "Miejscowość lokalizacji przedsięwzięcia deweloperskiego lub zadania inwestycyjnego", 
        "Ulica lokalizacji przedsięwzięcia deweloperskiego lub zadania inwestycyjnego", 
        "Nr nieruchomości lokalizacji przedsięwzięcia deweloperskiego lub zadania inwestycyjnego", 
        "Kod pocztowy lokalizacji przedsięwzięcia deweloperskiego lub zadania inwestycyjnego", 
        "Rodzaj nieruchomości: lokal mieszkalny, dom jednorodzinny", 
        "Nr lokalu lub domu jednorodzinnego nadany przez dewelopera", 
        "Cena m 2 powierzchni użytkowej lokalu mieszkalnego / domu jednorodzinnego [zł]", 
        "Data od której cena obowiązuje cena m 2 powierzchni użytkowej lokalu mieszkalnego / domu jednorodzinnego", 
        "Cena lokalu mieszkalnego lub domu jednorodzinnego będących przedmiotem umowy stanowiąca iloczyn ceny m2 oraz powierzchni [zł]", 
        "Data od której cena obowiązuje cena lokalu mieszkalnego lub domu jednorodzinnego będących przedmiotem umowy stanowiąca iloczyn ceny m2 oraz powierzchni", 
        "Cena lokalu mieszkalnego lub domu jednorodzinnego uwzględniająca cenę lokalu stanowiącą iloczyn powierzchni oraz metrażu i innych składowych ceny, o których mowa w art. 19a ust. 1 pkt 1), 2) lub 3) [zł]", 
        "Data od której obowiązuje cena lokalu mieszkalnego lub domu jednorodzinnego uwzględniająca cenę lokalu stanowiącą iloczyn powierzchni oraz metrażu i innych składowych ceny, o których mowa w art. 19a ust. 1 pkt 1), 2) lub 3)", 
        "Rodzaj części nieruchomości będących przedmiotem umowy", 
        "Oznaczenie części nieruchomości nadane przez dewelopera", 
        "Cena części nieruchomości, będących przedmiotem umowy [zł]", 
        "Data od której obowiązuje cena części nieruchomości, będących przedmiotem umowy", 
        "Rodzaj pomieszczeń przynależnych, o których mowa w art. 2 ust. 4 ustawy z dnia 24 czerwca 1994 r. o własności lokali", 
        "Oznaczenie pomieszczeń przynależnych, o których mowa w art. 2 ust. 4 ustawy z dnia 24 czerwca 1994 r. o własności lokali", 
        "Wyszczególnienie cen pomieszczeń przynależnych, o których mowa w art. 2 ust. 4 ustawy z dnia 24 czerwca 1994 r. o własności lokali [zł]", 
        "Data od której obowiązuje cena wyszczególnionych pomieszczeń przynależnych, o których mowa w art. 2 ust. 4 ustawy z dnia 24 czerwca 1994 r. o własności lokali", 
        "Wyszczególnienie praw niezbędnych do korzystania z lokalu mieszkalnego lub domu jednorodzinnego", 
        "Wartość praw niezbędnych do korzystania z lokalu mieszkalnego lub domu jednorodzinnego [zł]", 
        "Data od której obowiązuje cena wartości praw niezbędnych do korzystania z lokalu mieszkalnego lub domu jednorodzinnego", 
        "Wyszczególnienie rodzajów innych świadczeń pieniężnych, które nabywca zobowiązany jest spełnić na rzecz dewelopera w wykonaniu umowy przenoszącej własność", 
        "Wartość innych świadczeń pieniężnych, które nabywca zobowiązany jest spełnić na rzecz dewelopera w wykonaniu umowy przenoszącej własność [zł]", 
        "Data od której obowiązuje cena wartości innych świadczeń pieniężnych, które nabywca zobowiązany jest spełnić na rzecz dewelopera w wykonaniu umowy przenoszącej własność", 
        "Adres strony internetowej, pod którym dostępny jest prospekt informacyjny"
    };

    // Dodajemy nagłówki do pliku CSV
    csvRows.Add(string.Join(";", headers));

    // 3. Przejście po wynikach z GenerateReportAsync
    foreach (var row in rows)
    {
        var csvRow = new List<string>
        {
            // Deweloper
            row.DeveloperName ?? "x", 
            row.LegalForm ?? "x",
            row.KRS ?? "x",
            row.CEIDGNumber ?? "x",
            row.NIP ?? "x",
            row.REGON ?? "x",
            row.Phone ?? "x",
            row.Email ?? "x",
            row.Fax ?? "x", // Tutaj sprawdzamy, czy wartość istnieje, a jeśli nie, wstawiamy "x"
            row.Website ?? "x", 

            // Adres siedziby
            row.HeadquartersProvince ?? "x",
            row.HeadquartersCounty ?? "x",
            row.HeadquartersMunicipality ?? "x",
            row.HeadquartersCity ?? "x",
            row.HeadquartersStreet ?? "x",
            row.HeadquartersBuildingNumber ?? "x",
            row.HeadquartersApartmentNumber ?? "x",
            row.HeadquartersPostalCode ?? "x",
            row.HeadquartersProvince ?? "x",
            row.HeadquartersCounty ?? "x",
            row.HeadquartersMunicipality ?? "x",
            row.HeadquartersCity ?? "x",
            row.HeadquartersStreet ?? "x",
            row.HeadquartersBuildingNumber ?? "x",
            row.HeadquartersApartmentNumber ?? "x",
            row.HeadquartersPostalCode ?? "x",
            "X",
            row.ContactMethod ?? "x",

            // Inwestycja / lokalizacja
            
            //row.InvestmentName ?? "x",
            row.InvestmentProvince ?? "x",
            row.InvestmentCounty ?? "x", 
            row.InvestmentMunicipality ?? "x",
            row.InvestmentCity ?? "x",
            row.InvestmentStreet ?? "x",
            row.InvestmentBuildingNumber ?? "x",
            row.InvestmentPostalCode ?? "x",

            // Nieruchomość
            row.PropertyType ?? "x",
            row.ApartmentNumber ?? "x",
            //row.Area?.ToString() ?? "x",
            row.PricePerM2?.ToString() ?? "x",
            row.PriceFromDate?.ToString("yyyy-MM-dd") ?? "x",//////////////
            row.TotalPrice?.ToString() ?? "x",
            row.PriceFromDate?.ToString("yyyy-MM-dd") ?? "x",
            row.FullPrice?.ToString() ?? "x",
            row.PriceFromDate?.ToString("yyyy-MM-dd") ?? "x",
            
            //row.PriceFromDate?.ToString("yyyy-MM-dd") ?? "x",

            // Dodatki
            row.AttachedParts ?? "x",
            row.AttachedPartsLabels ?? "x",
            row.AttachedPartsPrice?.ToString() ?? "x",
            row.AttachedPartsPriceFromDate?.ToString("yyyy-MM-dd") ?? "x",
            "x","x","x","x","x","x","x","x","x","x",

            // Link do prospektu
            row.ProspectUrl ?? "x"
        };

        // Dodajemy wiersz do CSV
        csvRows.Add(string.Join(";", csvRow));
    }

    // Ścieżka do pliku CSV
    string outputDir = Path.Combine(_env.WebRootPath, "dane");
    Directory.CreateDirectory(outputDir);

    string filePath = Path.Combine(outputDir, "cennik.csv");


    // Zapisz plik CSV
    File.WriteAllText(filePath, string.Join("\n", csvRows));

    Console.WriteLine($"CSV file generated at: {filePath}");

    return filePath;
}







        // 🔧 Pomocnicza metoda do generowania linków prospektów
        private static string GenerateProspectUrl(string baseUrl, string investmentName)
        {
            if (string.IsNullOrWhiteSpace(baseUrl) || string.IsNullOrWhiteSpace(investmentName))
                return baseUrl ?? string.Empty;

            string slug = investmentName
                .ToLower()
                .Trim()
                .Replace(" ", "_")
                .Replace("ł", "l").Replace("ś", "s").Replace("ć", "c")
                .Replace("ń", "n").Replace("ź", "z").Replace("ż", "z")
                .Replace("ó", "o").Replace("ą", "a").Replace("ę", "e");

            return $"{baseUrl.TrimEnd('/')}/{slug}";
        }
    }
}
