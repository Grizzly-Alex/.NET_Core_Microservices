namespace Mango.Services.OrderAPI.Models.Dto
{
    public sealed class StripeRequestDto
    {
        public string StripeSessionUrl { get; set; }    
        public string StripeSessionId { get; set;}
        public string ApprovedUrl { get; set;}  
        public string CancelUrl { get; set;}    
        public OrderHeaderDto OrderHeader { get; set;}
    }
}
