using RabbitMQ.Client;

namespace Logic.Services
{
    public abstract class BaseChannelService : IDisposable
    {
        private readonly string _hostName;
        protected IModel _channel;
        private IConnection _connection;
        private string _channelName;

        public BaseChannelService(string hostName)
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

        public void Dispose()
        {
            _channel.Close();
            _connection.Close();
            _connection.Dispose();
            _channel.Dispose();
        }
    }
}
