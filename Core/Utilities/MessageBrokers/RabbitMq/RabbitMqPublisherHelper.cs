using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Core.Utilities.MessageBrokers.RabbitMq
{
    public class RabbitMqPublisherHelper
    {
        readonly IConfiguration _configuration;
        readonly RabbitMqOptions _rabbitMqOptions;

        public RabbitMqPublisherHelper(IConfiguration configuration)
        {
            _configuration = configuration;
            _rabbitMqOptions = _configuration.GetSection("RabbitMq").Get<RabbitMqOptions>();
        }

        public void Publish<T>(T message, string exchange, string routingKey)
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
                var stringMessage = JsonSerializer.Serialize(message);
                var body = Encoding.UTF8.GetBytes(stringMessage);

                channel.BasicPublish(
                    exchange: exchange,
                    routingKey: routingKey,
                    body: body
                    );
            }
        }
    }
}
