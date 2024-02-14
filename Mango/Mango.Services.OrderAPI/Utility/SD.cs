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
        public static string OrderCreatedTopic { get; set; }
        public static string RewardsUpdateQueue { get; set; }
        public static string EmailUpdateQueue { get; set; }
        public static string RoutingKeyForRewardsUpdateQueue { get; set; }
        public static string RoutingKeyEmailUpdateQueue { get; set; }

        public static void Initializing(IConfiguration config)
        {
            StripeSessionKey = config["Stripe:SecretKey"];
            OrderCreatedTopic = config["TopicAndQueueNames:OrderCreatedTopic"];
            RewardsUpdateQueue = config["TopicAndQueueNames:RewardsUpdateQueue"];
            EmailUpdateQueue = config["TopicAndQueueNames:EmailUpdateQueue"];
            RoutingKeyForRewardsUpdateQueue = config["TopicAndQueueNames:RoutingKeyForRewardsUpdateQueue"];
            RoutingKeyEmailUpdateQueue = config["TopicAndQueueNames:RoutingKeyEmailUpdateQueue"];
        }
    }
}
