using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using CsvHelper;
using CsvHelper.TypeConversion;
using System;
using System.Globalization;

public class DecimalNullableConverter : DecimalConverter
{
    public override object? ConvertFromString(string? text, IReaderRow row, MemberMapData memberMapData)
    {
        if (string.IsNullOrWhiteSpace(text))
            return null;

        text = text.Trim().Replace("\"", "");

        var invalidValues = new[] { "x", "-", "brak", "null", "n/a" };
        if (invalidValues.Contains(text.ToLowerInvariant()))
            return null;

        // Wartości typu "Parking hala", "Mieszkanie", itd:
        if (!decimal.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out var value))
            return null;

        return value;
    }
}


public class DeveloperDecimalConverter : DecimalConverter
{
    public override object ConvertFromString(string? text, IReaderRow row, MemberMapData memberMapData)
    {
        if (string.IsNullOrWhiteSpace(text))
            return null;

        // X, -, brak wartości → null
        if (text.Trim() == "X" || text.Trim() == "-" || text.Trim() == "x")
            return null;

        // próba konwersji
        if (decimal.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out var value))
            return value;

        return null;
    }
}