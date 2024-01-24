namespace Mango.Services.OrderAPI.Models.Dto
{
    public sealed class OrderHeaderDto
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public string? CouponCode { get; set; }
        public double Discount { get; set; }
        public double Total { get; set; }
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public DateTime Time { get; set; }
        public string? Status { get; set; }
        public string? PaymentIntentId { get; set; }
        public string? StriprSessionId { get; set; }

        IEnumerable<OrderDetails> OrderDetails { get; set; }
    }
}
