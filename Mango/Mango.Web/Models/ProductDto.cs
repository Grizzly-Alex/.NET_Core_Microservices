using System.ComponentModel.DataAnnotations;

namespace Mango.Web.Models
{
    public sealed class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
		public string? ImageUrl { get; set; }
		public string? ImageLocalPath { get; set; }
		[Range(1, 100)]
        public int Count { get; set; } = 1;
        public IFormFile? Imgae { get; set; }
    }
}
