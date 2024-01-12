namespace Mango.Web.Models
{
    public sealed class CartDto
    {
        public CartHeaderDto CartHeader { get; set; }
        public IEnumerable<CartDetailsDto>? CartDetails { get; set; }
    }
}
