namespace apitank.DTOs
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? Key { get; set; }
        public string? Response { get; set; }
    }
}
