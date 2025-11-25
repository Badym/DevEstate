using CsvHelper;
using CsvHelper.Configuration;
using DevEstate.Dtos.DeveloperOpenData;
using System.Globalization;
using DevEstate.Services.DeveloperOpenData;

public class CsvDataParser
{
    public List<DeveloperPriceRecord> ParseCsv(string filePath)
    {
        string delimiter = DetectDelimiter(filePath);

        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Delimiter = delimiter,
            HasHeaderRecord = true,
            BadDataFound = null,
            MissingFieldFound = null,
            TrimOptions = TrimOptions.Trim,

            PrepareHeaderForMatch = args =>
                args.Header
                    .Replace("\uFEFF", "")     // usuń BOM
                    .Trim()
                    .Trim('"')
                    .Trim('\'')
                    .Replace("  ", " ")
                    .ToLowerInvariant()
        };

        using var reader = new StreamReader(filePath);
        using var csv = new CsvReader(reader, config);

        csv.Context.RegisterClassMap<DeveloperPriceRecordClassMap>();

        try
        {
            return csv.GetRecords<DeveloperPriceRecord>().ToList();
        }
        catch (Exception ex)
        {
            Console.WriteLine("❌ ERROR parsing CSV: " + ex);
            return new List<DeveloperPriceRecord>();
        }
    }

    private string DetectDelimiter(string filePath)
    {
        string firstLine = File.ReadLines(filePath).FirstOrDefault() ?? "";

        // jeśli linia zawiera średnik — zwykle oznacza CSV z delimiterem ;
        // bo przecinki często są częścią wartości
        if (firstLine.Contains(';'))
        {
            // ale sprawdźmy czy liczba "pól" po splitowaniu ma sens
            var semicolonParts = firstLine.Split(';');

            if (semicolonParts.Length > 5) // csv deweloperskie mają >50 kolumn
                return ";";
        }

        // sprawdźmy przecinek
        var commaParts = firstLine.Split(',');

        if (commaParts.Length > 5)
        {
            return ",";
        }

        // fallback
        return ";";
    }

}