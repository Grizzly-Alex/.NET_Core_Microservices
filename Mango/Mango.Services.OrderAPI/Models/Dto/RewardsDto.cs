namespace Mango.Services.OrderAPI.Models.Dto
{
    public sealed class RewardsDto
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int RewardsActivity { get; set; }
        public int OrderId { get; set; }
    }
}
