namespace Mango.Services.OrderAPI.RabbitMQSender
{
    public interface IMessageSender
    {
        void SendMessage(object message, string exchangeName);
    }
}
