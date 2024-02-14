namespace Mango.Services.ShoppingCartAPI.RabbitMQSender
{
    public interface IMessageSender
    {
        void SendMessage(object message, string queueName);
    }
}
