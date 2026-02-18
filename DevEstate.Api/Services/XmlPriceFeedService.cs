using System.Text;
using System.Xml.Linq;
using DevEstate.Api.Dtos;
using DevEstate.Api.Repositories;
using ZstdSharp.Unsafe;

namespace DevEstate.Api.Services;

public class XmlPriceFeedService
    {
        private readonly CompanyRepository _companyRepo;

        private readonly XmlDatasetSettingsDto _settings;

        public XmlPriceFeedService(CompanyRepository companyRepo, XmlDatasetSettingsDto settings)
        {
            _companyRepo = companyRepo;
            _settings = settings;
        }

        public  async Task<string> GenerateXml(XmlResourceInfoDto resource, string outputDir)
        {
            var companyEntity = (await _companyRepo.GetAllAsync()).FirstOrDefault()
                                ?? throw new Exception("Company data not found");

            var company = new CompanyDetailsDtos.ResponseCompanyDetails
            {
                Id = companyEntity.Id,
                Name = companyEntity.Name,
                LegalForm = companyEntity.LegalForm,
                KRS = companyEntity.KRS,
                CEIDGNumber = companyEntity.CEIDGNumber,
                NIP = companyEntity.NIP,
                REGON = companyEntity.REGON,
                Phone = companyEntity.Phone,
                Fax = companyEntity.Fax,
                Email = companyEntity.Email,
                Website = companyEntity.Website,
                Province = companyEntity.Province,
                County = companyEntity.County,
                Municipality = companyEntity.Municipality,
                City = companyEntity.City,
                Street = companyEntity.Street,
                BuildingNumber = companyEntity.BuildingNumber,
                ApartmentNumber = companyEntity.ApartmentNumber,
                PostalCode = companyEntity.PostalCode,
                Description = companyEntity.Description,
                LogoImage = companyEntity.LogoImage,
                ContactMethond = companyEntity.ContactMethod // uwaga na literówkę w DTO
            };

            XNamespace ns = "urn:otwarte-dane:harvester:1.13";

            var xml = new XElement(ns + "datasets",
                new XElement("dataset",
                    new XAttribute("status", "published"),

                    new XElement("extIdent", _settings.DatasetExtIdent),

                    new XElement("title",
                        new XElement("polish", $"Ceny ofertowe mieszkań dewelopera {company.Name} w {resource.DataDate.Year} r."),
                        new XElement("english", $"Offer prices of apartments of developer {company.Name} in {resource.DataDate.Year}.")
                    ),

                    new XElement("description",
                        new XElement("polish", $"Zbiór danych zawiera informacje o cenach ofertowych mieszkań dewelopera {company.Name}."),
                        new XElement("english", $"Dataset contains offer prices of developer {company.Name}.")
                    ),

                    new XElement("updateFrequency", "daily"),
                    new XElement("hasDynamicData", false),
                    new XElement("hasHighValueData", true),
                    new XElement("hasHighValueDataFromEuropeanCommissionList", false),
                    new XElement("hasResearchData", false),

                    new XElement("categories",
                        new XElement("category", "ECON")
                    ),

                    new XElement("resources",
                        new XElement("resource",
                            new XAttribute("status", "published"),

                            new XElement("extIdent", GenerateResourceExtIdent(resource.DataDate)),

                            new XElement("url", resource.CsvUrl),

                            new XElement("title",
                                new XElement("polish", $"Ceny ofertowe mieszkań {company.Name} {resource.DataDate:yyyy-MM-dd}"),
                                new XElement("english", $"Offer prices {company.Name} {resource.DataDate:yyyy-MM-dd}")
                            ),

                            new XElement("description",
                                new XElement("polish", $"Cennik udostępniony dnia {resource.DataDate:yyyy-MM-dd}"),
                                new XElement("english", $"Price list published {resource.DataDate:yyyy-MM-dd}")
                            ),

                            new XElement("availability", "local"),
                            new XElement("dataDate", resource.DataDate.ToString("yyyy-MM-dd")),

                            new XElement("specialSigns",
                                new XElement("specialSign", "X")
                            ),

                            new XElement("hasDynamicData", false),
                            new XElement("hasHighValueData", true),
                            new XElement("hasHighValueDataFromEuropeanCommissionList", false),
                            new XElement("hasResearchData", false),
                            new XElement("containsProtectedData", false)
                        )
                    ),

                    new XElement("tags",
                        new XElement("tag", new XAttribute("lang", "pl"), "Deweloper")
                    )
                )
            );

            string xmlPath = Path.Combine(outputDir, "cennik.xml");
            File.WriteAllText(xmlPath, xml.ToString(), Encoding.UTF8);

            return xmlPath;
        }

        private string GenerateResourceExtIdent(DateTime dataDate)
        {
            var yyyymmdd = dataDate.ToString("yyyyMMdd");          // np. 20260218
            var seed = $"{yyyymmdd}-DevEstate";                // "sól" żeby nie było zbyt "gołe"

            using var md5 = System.Security.Cryptography.MD5.Create();
            var bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(seed)); // 16 bajtów

            return new Guid(bytes).ToString("D"); // ✅ 36 znaków
        }
    }   