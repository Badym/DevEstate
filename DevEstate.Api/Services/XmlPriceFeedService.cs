using System.Text;
using System.Xml.Linq;
using DevEstate.Api.Dtos;

namespace DevEstate.Api.Services;

public class XmlPriceFeedService
    {
        private readonly CompanyDtos.CompanyDto _company;
        private readonly XmlDatasetSettingsDto _settings;

        public XmlPriceFeedService(CompanyDtos.CompanyDto company, XmlDatasetSettingsDto settings)
        {
            _company = company;
            _settings = settings;
        }

        public string GenerateXml(XmlResourceInfoDto resource, string outputDir)
        {
            XNamespace ns = "urn:otwarte-dane:harvester:1.13";

            var xml = new XElement(ns + "datasets",
                new XElement("dataset",
                    new XAttribute("status", "published"),

                    new XElement("extIdent", _settings.DatasetExtIdent),

                    new XElement("title",
                        new XElement("polish", $"Ceny ofertowe mieszkań dewelopera {_company.Name} w {resource.DataDate.Year} r."),
                        new XElement("english", $"Offer prices of apartments of developer {_company.Name} in {resource.DataDate.Year}.")
                    ),

                    new XElement("description",
                        new XElement("polish", $"Zbiór danych zawiera informacje o cenach ofertowych mieszkań dewelopera {_company.Name}."),
                        new XElement("english", $"Dataset contains offer prices of developer {_company.Name}.")
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

                            new XElement("extIdent", GenerateResourceExtIdent()),

                            new XElement("url", resource.CsvUrl),

                            new XElement("title",
                                new XElement("polish", $"Ceny ofertowe mieszkań {_company.Name} {resource.DataDate:yyyy-MM-dd}"),
                                new XElement("english", $"Offer prices {_company.Name} {resource.DataDate:yyyy-MM-dd}")
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

        private string GenerateResourceExtIdent()
        {
            return Guid.NewGuid().ToString("D");
        }
    }   