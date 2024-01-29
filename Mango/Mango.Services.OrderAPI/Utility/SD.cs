namespace Mango.OrderAPI.Utility
{
    public sealed class SD
    {
        public const string StatusPending = "Pending";
        public const string StatusApproved = "Approved";
        public const string StatusReadyForPickup = "ReadyForPickup";
        public const string StatusCompleted = "Completed";
        public const string StatusRefunded = "Refunded";
        public const string StatusCancelled = "Cancelled";

        public const string RoleAdmin = "ADMIN";
        public const string RoleCustomer = "CUSTOMER";

        public static string StripeSessionKey {  get; set; }

        public static void Initializing(IConfiguration config)
        {
            StripeSessionKey = config["Stripe:SecretKey"];
        }
    }
}
