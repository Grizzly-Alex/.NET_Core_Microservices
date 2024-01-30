namespace Mango.RewardsAPI.Utility
{
    public sealed class SD
    {
        public static string OrderCreatedTopic {  get; set; }

        public static void Initializing(IConfiguration config)
        {
            OrderCreatedTopic = config["TopicAndQueueNames:OrderCreatedTopic"];
        }
    }
}
