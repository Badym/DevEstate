using System.ComponentModel;
using OfficeOpenXml;
using CsvHelper;
using CsvHelper.Configuration;
using DevEstate.Dtos.DeveloperOpenData;
using System.Globalization;

public class ExcelDataParser
{
    public List<DeveloperPriceRecord> ParseXlsx(string filePath)
    {
        ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

        var records = new List<DeveloperPriceRecord>();

        using var package = new ExcelPackage(new FileInfo(filePath));
        var sheet = package.Workbook.Worksheets.First();

        // Zakładamy, że pierwszy wiersz = nagłówki
        int rowCount = sheet.Dimension.Rows;
        int colCount = sheet.Dimension.Columns;

        // Wyświetlenie nagłówków w konsoli
        for (int col = 1; col <= colCount; col++)
        {
            string header = sheet.Cells[1, col].Text.Trim();
            Console.WriteLine($"Header {col}: {header}");  // Logowanie nagłówków
        }

        // zrobimy mapę indeks → nagłówek (po trimach tak jak w CSV)
        Dictionary<string, int> headerMap = new();

        for (int col = 1; col <= colCount; col++)
        {
            string header = sheet.Cells[1, col].Text
                .Trim()
                .ToLowerInvariant()
                .Replace("  ", " ");

            if (!headerMap.ContainsKey(header))
                headerMap.Add(header, col);
        }

        // Przetwarzamy dane wiersz po wierszu
        for (int row = 2; row <= rowCount; row++)
        {
            var record = new DeveloperPriceRecord
            {
                Wojewodztwo = GetString(sheet, row, headerMap, new[]
                {
                    "województwo lokalizacji przedsięwzięcia deweloperskiego lub zadania inwestycyjnego",
                    "lokalizacja przedsięwzięcia deweloperskiego lub zadania inwestycyjnego - województwo",
                    "województwo",
                    "region"
                }),

                Powiat = GetString(sheet, row, headerMap, new[]
                {
                    "powiat lokalizacji przedsięwzięcia deweloperskiego lub zadania inwestycyjnego",
                    "lokalizacja przedsięwzięcia deweloperskiego lub zadania inwestycyjnego - powiat",
                    "powiat"
                }),

                Gmina = GetString(sheet, row, headerMap, new[]
                {
                    "gmina lokalizacji przedsięwzięcia deweloperskiego lub zadania inwestycyjnego",
                    "lokalizacja przedsięwzięcia deweloperskiego lub zadania inwestycyjnego - gmina",
                    "gmina"
                }),

                Miejscowosc = GetString(sheet, row, headerMap, new[]
                {
                    "miejscowość lokalizacji przedsięwzięcia deweloperskiego lub zadania inwestycyjnego",
                    "lokalizacja przedsięwzięcia deweloperskiego lub zadania inwestycyjnego - miejscowość",
                    "miejscowość",
                    "miejscowosc"
                }),

                CenaZaM2 = GetDecimal(sheet, row, headerMap, new[]
                {
                    "cena za m2 nieruchomości",
                    "cena m2",
                    "cena m 2",
                    "cena m 2 powierzchni użytkowej lokalu mieszkalnego / domu jednorodzinnego [zł]"
                })
            };

            records.Add(record);
        }

        return records;
    }

    private string? GetString(ExcelWorksheet sheet, int row, Dictionary<string, int> map, string[] headers)
    {
        foreach (var h in headers)
        {
            if (map.TryGetValue(h.ToLowerInvariant(), out int col))
            {
                var val = sheet.Cells[row, col].Text?.Trim();
                if (string.IsNullOrWhiteSpace(val) || val == "x" || val == "-" || val == "null")
                    return null;

                return val;
            }
        }

        return null;
    }

    private decimal? GetDecimal(ExcelWorksheet sheet, int row, Dictionary<string, int> map, string[] headers)
    {
        foreach (var h in headers)
        {
            if (map.TryGetValue(h.ToLowerInvariant(), out int col))
            {
                var val = sheet.Cells[row, col].Text?.Trim();

                if (string.IsNullOrWhiteSpace(val) || val == "x" || val == "-" || val == "null")
                    return null;

                if (decimal.TryParse(val, NumberStyles.Any, CultureInfo.InvariantCulture, out var d))
                    return d;

                return null;
            }
        }

        return null;
    }
}

