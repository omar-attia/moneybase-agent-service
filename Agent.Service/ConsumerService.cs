using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using Agent.Service.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Agent.Service
{
    public class ConsumerService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IConfiguration _configuration;
        private readonly IModel _channel;
        private readonly string _queueName;

        public ConsumerService(IServiceScopeFactory serviceScopeFactory, IConfiguration configuration)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _configuration = configuration;

            var factory = new ConnectionFactory()
            {
                HostName = _configuration["RabbitMq:HostName"],
                UserName = _configuration["RabbitMq:UserName"],
                Password = _configuration["RabbitMq:Password"]
            };

            var connection = factory.CreateConnection();
            _channel = connection.CreateModel();

            _queueName = _configuration["RabbitMq:Queues:ConsumeQueue"];
            _channel.QueueDeclare(queue: _queueName,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);
        }

        // Start consuming and pass the message handler as a parameter
        public void StartConsuming()
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);


                // Process the message using the passed handler
                using var scope = _serviceScopeFactory.CreateScope();
                var scopedService = scope.ServiceProvider.GetRequiredService<IAgentAssignmentService>();
                await scopedService.ListenForSessions(message);

                // Acknowledge the message
                _channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
            };

            _channel.BasicConsume(queue: _queueName,
                autoAck: false,
                consumer: consumer);
        }
    }
}
