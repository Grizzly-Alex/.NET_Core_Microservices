namespace Mango.EmailAPI.Utility
{
    public sealed class SD
    {
        public static string EmailShoppingCart { get; set; }
        public static string RegisterUser { get; set; }
        public static string OrderCreated { get; set; }
        public static string OrderCreatedEmail { get; set; }

        public static void Initializing(IConfiguration config)
        {
            EmailShoppingCart = config["TopicAndQueueNames:EmailShoppingCartQueue"];
            RegisterUser = config["TopicAndQueueNames:RegisterUserQueue"];
            OrderCreated = config["TopicAndQueueNames:OrderCreatedTopic"];
            OrderCreatedEmail = config["TopicAndQueueNames:OrderCreatedEmailSubscription"];
        }
    }
}
