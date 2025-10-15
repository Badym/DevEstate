namespace DevEstate.Api.Dtos;

public static class PriceHistoryDtos
{
    public class Create
    {
        public string PropertyId { get; set; } = null!;
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public decimal NewPrice { get; set; }
    }

    public class Response
    {
        public string Id { get; set; } = null!;
        public string PropertyId { get; set; } = null!;
        public DateTime Date { get; set; }
        public decimal NewPrice { get; set; }
    }
}