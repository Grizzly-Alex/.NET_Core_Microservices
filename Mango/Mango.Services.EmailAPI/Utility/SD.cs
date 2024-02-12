namespace Mango.EmailAPI.Utility
{
    public sealed class SD
    {
        public static string OrderCreatedTopic { get; set; }
        public static string OrderCreatedEmailUpdateQueue { get; set; }
        public static string EmailShoppingCartQueue { get; set; }
        public static string RegisterUserQueue { get; set; }
        public static string RoutingKeyForUpdateQueue { get; set; }

        public static void Initializing(IConfiguration config)
        {
            EmailShoppingCartQueue = config["TopicAndQueueNames:EmailShoppingCartQueue"];
            RegisterUserQueue = config["TopicAndQueueNames:RegisterUserQueue"];
            OrderCreatedTopic = config["TopicAndQueueNames:OrderCreatedTopic"];
            OrderCreatedEmailUpdateQueue = config["TopicAndQueueNames:OrderCreatedEmailUpdateQueue"];
            RoutingKeyForUpdateQueue = config["TopicAndQueueNames:RoutingKeyForUpdateQueue"];
        }
    }
}
