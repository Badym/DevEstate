using DevEstate.Dtos.DeveloperOpenData;

public class DeveloperOpenDataParser
{
    public List<DeveloperPriceRecord> Parse(string filePath)
    {
        if (filePath.EndsWith(".csv")) return new CsvDataParser().ParseCsv(filePath);
        if (filePath.EndsWith(".xlsx")) return new ExcelDataParser().ParseXlsx(filePath);
        
        throw new InvalidOperationException("Unsupported file format.");
    }
}
