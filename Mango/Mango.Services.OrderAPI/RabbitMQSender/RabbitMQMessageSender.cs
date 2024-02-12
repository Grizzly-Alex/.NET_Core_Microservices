using Mango.OrderAPI.Utility;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace Mango.Services.OrderAPI.RabbitMQSender
{
    public sealed class RabbitMQMessageSender : IMessageSender
    {
        private readonly string _hostName;
        private readonly string _userName;
        private readonly string _password;
        private IConnection _connection;


        public RabbitMQMessageSender()
        {
            _hostName = "localhost";
            _userName = "guest";
            _password = "guest";             
        }

        public void SendMessage(object message, string exchangeName)
        {
            if (ConnectionExists())
            {
                using var channel = _connection.CreateModel();
                channel.ExchangeDeclare(exchangeName, ExchangeType.Direct, durable:false);
                channel.QueueDeclare(SD.EmailUpdateQueue, false, false, false, null);
                channel.QueueDeclare(SD.RewardsUpdateQueue, false, false, false, null);

                channel.QueueBind(SD.EmailUpdateQueue, exchangeName, SD.RoutingKeyEmailUpdateQueue);
                channel.QueueBind(SD.RewardsUpdateQueue, exchangeName, SD.RoutingKeyForRewardsUpdateQueue);

                var json = JsonConvert.SerializeObject(message);
                var body = Encoding.UTF8.GetBytes(json);

                channel.BasicPublish(exchange: exchangeName, routingKey: SD.RoutingKeyEmailUpdateQueue, null, body: body);
                channel.BasicPublish(exchange: exchangeName, routingKey: SD.RoutingKeyForRewardsUpdateQueue, null, body: body);
            }
        }

        private void CreateConnection()
        {
            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = _hostName,
                    Password = _password,
                    UserName = _userName
                };

                _connection = factory.CreateConnection();
            }
            catch (Exception ex)
            {

            }
        }

        private bool ConnectionExists()
        {
            if (_connection != null)
            {
                return true;
            }
            CreateConnection();
            return true;
        }
    }
}
