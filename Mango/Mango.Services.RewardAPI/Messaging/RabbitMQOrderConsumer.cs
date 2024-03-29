﻿using Mango.Services.RewardAPI.Services;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using Mango.RewardAPI.Utility;
using Mango.Services.RewardAPI.Message;

namespace Mango.Services.EmailAPI.Messaging
{
    public class RabbitMQOrderConsumer : BackgroundService
    {   
        private readonly RewardService _rewardService;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public RabbitMQOrderConsumer(RewardService rewardService)
        {
            _rewardService = rewardService;

            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                Password = "guest",
                UserName = "guest",
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(SD.OrderCreatedTopic, ExchangeType.Direct);

            _channel.QueueDeclare(SD.OrderCreatedRewardsUpdateQueue, false, false, false, null);

            _channel.QueueBind(SD.OrderCreatedRewardsUpdateQueue, SD.OrderCreatedTopic, SD.RoutingKeyForUpdateQueue);
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

            _channel.BasicConsume(SD.OrderCreatedRewardsUpdateQueue, false, consumer);

            return Task.CompletedTask;
        }

        private async Task HandleMessage(RewardsMessage rewardsMessage)
        {
           await _rewardService.UpdateRewards(rewardsMessage);
        }
    }
}
