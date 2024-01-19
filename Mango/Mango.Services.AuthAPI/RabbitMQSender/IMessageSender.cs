namespace Mango.Services.AuthAPI.RabbitMQSender
{
    public interface IMessageSender
    {
        void SendMessage(object message, string queueName);
    }
}
