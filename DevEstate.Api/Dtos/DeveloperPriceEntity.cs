namespace DevEstate.Dtos.DeveloperOpenData
{
    public class DeveloperPriceRecordAggregatedDto
    {
        public string? Wojewodztwo { get; set; }
        public string? Powiat { get; set; }
        public string? Gmina { get; set; }
        public string? Miejscowosc { get; set; }

        public decimal? CenaZaM2 { get; set; }

        // NOWE POLA — dla agregacji
        public int Ilosc { get; set; }        // liczba rekordów
        public decimal? Suma { get; set; }    // suma cen za m²
    }
    
    public class DeveloperPriceRecordUpdateDto
    {
        public string? Wojewodztwo { get; set; }
        public string? Powiat { get; set; }
        public decimal? CenaZaM2 { get; set; }
    }
}