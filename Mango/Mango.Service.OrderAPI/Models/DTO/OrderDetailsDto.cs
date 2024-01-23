using Mango.Services.OrderAPI.Models.Dto;

namespace Mango.Service.OrderAPI.Models.Dto
{
    public sealed class OrderDetailsDto
    {

        public int Id { get; set; }
        public int CartHeaderId { get; set; }
        public int ProductId { get; set; }
        public ProductDto? Product { get; set; }
        public int Count { get; set; }
        public string ProductName { get; set; }
        public double Price { get; set; }
    }
}
