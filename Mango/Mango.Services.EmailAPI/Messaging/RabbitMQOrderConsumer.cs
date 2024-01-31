using Mango.EmailAPI.Utility;
using Mango.Services.EmailAPI.Message;
using Mango.Services.EmailAPI.Services;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Mango.Services.EmailAPI.Messaging
{
    public class RabbitMQOrderConsumer : BackgroundService
    {   
        private readonly EmailService _emailService;
        private readonly IConnection _connection;
        private IModel _channel;


        public RabbitMQOrderConsumer(EmailService emailService)
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
            _channel.ExchangeDeclare(SD.OrderCreated, ExchangeType.Fanout);

        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());
                RewardsMessage rewardsMessage = JsonConvert.DeserializeObject<RewardsMessage>(content);
                await HandleMessage(rewardsMessage);

                _channel.BasicAck(ea.DeliveryTag, false);
            };

            _channel.BasicConsume(SD.OrderCreated, false, consumer);

            return Task.CompletedTask;
        }

        private async Task HandleMessage(RewardsMessage rewardsMessage)
        {
           await _emailService.LogOrderPlaced(rewardsMessage);
        }
    }
}
