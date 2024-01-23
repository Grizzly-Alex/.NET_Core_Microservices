using Mango.EmailAPI.Utility;
using Mango.Services.EmailAPI.Models.Dto;
using Mango.Services.EmailAPI.Services;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Mango.Services.EmailAPI.Messaging
{
    public class RabbitMQCartConsumer : BackgroundService
    {   
        private readonly EmailService _emailService;
        private readonly IConnection _connection;
        private IModel _channel;

        public RabbitMQCartConsumer(EmailService emailService)
        {
            _emailService = emailService;

            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                Password = "guest",
                UserName = "guest",
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(SD.EmailShoppingCart, false, false, false, null); ;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());
                CartDto cart = JsonConvert.DeserializeObject<CartDto>(content);
                HandleMessage(cart).GetAwaiter().GetResult();

                _channel.BasicAck(ea.DeliveryTag, false);
            };

            _channel.BasicConsume(SD.EmailShoppingCart, false, consumer);

            return Task.CompletedTask;
        }

        private async Task HandleMessage(CartDto cart)
        {
            _emailService.EmailCartAndLog(cart).GetAwaiter().GetResult();
        }
    }
}
