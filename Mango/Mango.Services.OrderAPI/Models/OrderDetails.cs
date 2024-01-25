using Mango.Services.OrderAPI.Models.Dto;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mango.Services.OrderAPI.Models
{
    public sealed class OrderDetails
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public int CartHeaderId { get; set; }
        [ForeignKey("OrderHeaderId")]
        public OrderHeader? OrderHeader { get; set; }
        public int ProductId { get; set; }
        [NotMapped]
        public ProductDto? Product { get; set; }
        public int Count { get; set; }

        public string ProductName { get; set; } 
        public double Price { get; set; }   
    }
}