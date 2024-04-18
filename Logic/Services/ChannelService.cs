using RabbitMQ.Client;
using System.Text;

namespace Logic.Services
{
    public class ChannelService
    {
        private readonly string _hostName;
        private IModel _channel;
        public ChannelService(string hostName)
        {
            _hostName = hostName;
        }

        public void CreateChannel(string channelName)
        {
            var factory = new ConnectionFactory { HostName = _hostName };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            _channel.QueueDeclare(queue: channelName,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);
        }

        public bool Send(string message, string channelName)
        {
            try
            {
                var body = Encoding.UTF8.GetBytes(message);

                _channel.BasicPublish(exchange: string.Empty,
                                     routingKey: channelName,
                                     basicProperties: null,
                                     body: body);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
