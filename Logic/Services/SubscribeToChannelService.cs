using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Logic.Services
{
    public class SubscribeToChannelService: IDisposable
    {
        private readonly string _hostName;
        private IModel _channel;
        private IConnection _connection;
        private string _channelName;
        public SubscribeToChannelService(string hostName)
        {
            _hostName = hostName;
        }

        public string ChannelName => _channelName;
        public IModel Channel => _channel;

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

        public bool Consume(EventingBasicConsumer consumer)
        {
            try
            {
                _channel.BasicConsume(queue: _channelName,
                                     autoAck: true,
                                     consumer: consumer);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void Dispose()
        {
            _connection.Dispose();
            _channel.Dispose();
        }
    }
}
