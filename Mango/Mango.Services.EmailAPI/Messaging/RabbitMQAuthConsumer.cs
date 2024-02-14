using Mango.EmailAPI.Utility;
using Mango.Services.EmailAPI.Services;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Mango.Services.EmailAPI.Messaging
{
    public class RabbitMQAuthConsumer : BackgroundService
    {   
        private readonly EmailService _emailService;
        private readonly IConnection _connection;
        private IModel _channel;

        public RabbitMQAuthConsumer(EmailService emailService)
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
            _channel.QueueDeclare(SD.RegisterUserQueue, false, false, false, null); ;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());
                String email = JsonConvert.DeserializeObject<string>(content);
                await HandleMessage(email);

                _channel.BasicAck(ea.DeliveryTag, false);
            };

            _channel.BasicConsume(SD.RegisterUserQueue, false, consumer);

            return Task.CompletedTask;
        }

        private async Task HandleMessage(string email)
        {
            await _emailService.RegisterUserEmailAndLog(email);
        }
    }
}
