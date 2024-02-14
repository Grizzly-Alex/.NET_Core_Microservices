namespace Mango.Services.OrderAPI.Models.Dto
{
    public sealed class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string? ImageUrl { get; set; }
        public int Count { get; set; } = 1;
    }
}
