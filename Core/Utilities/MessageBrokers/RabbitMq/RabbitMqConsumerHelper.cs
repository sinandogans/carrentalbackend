using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Core.Utilities.MessageBrokers.RabbitMq
{
    public class RabbitMqConsumerHelper
    {
        private readonly IConfiguration _configuration;
        private readonly RabbitMqOptions _rabbitMqOptions;

        public RabbitMqConsumerHelper(IConfiguration configuration)
        {
            _configuration = configuration;
            _rabbitMqOptions = _configuration.GetSection("MessageBrokerOptions").Get<RabbitMqOptions>();
        }

        public void Consume(string queue)
        {
            var factory = new ConnectionFactory
            {
                HostName = _rabbitMqOptions.HostName,
                UserName = _rabbitMqOptions.UserName,
                Password = _rabbitMqOptions.Password,
            };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                var consumer = new EventingBasicConsumer(channel);

                consumer.Received += (model, e) =>
                {
                    var body = e.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);

                    Console.WriteLine($"Message: {message}");
                };

                channel.BasicConsume(
                    queue: queue,
                    autoAck: false,
                    consumer: consumer);

            }
        }
    }
}
