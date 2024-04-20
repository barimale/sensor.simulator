using RabbitMQ.Client;
using System.Text;

namespace Logic.Services
{
    public class PublishToChannelService: IDisposable
    {
        private readonly string _hostName;
        private IModel _channel;
        private IConnection _connection;
        private string _channelName;

        public PublishToChannelService(string hostName)
        {
            _hostName = hostName;
        }

        public string ChannelName => _channelName;

        public void CreateChannel(string channelName)
        {
            _channelName = channelName;

            var factory = new ConnectionFactory { HostName = _hostName };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(queue: _channelName,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);
        }

        public bool Send(string message)
        {
            try
            {
                var body = Encoding.UTF8.GetBytes(message);

                _channel.BasicPublish(exchange: string.Empty,
                                     routingKey: _channelName,
                                     basicProperties: null,
                                     body: body);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void Dispose()
        {
            _channel.Close();
            _connection.Close();
            _connection.Dispose();
            _channel.Dispose();
        }
    }
}
