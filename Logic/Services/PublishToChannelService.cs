using RabbitMQ.Client;
using System.Text;

namespace Logic.Services
{
    public class PublishToChannelService : BaseChannelService
    {
        public PublishToChannelService(string hostName)
            : base(hostName)
        {
            // intentionally left blank
        }

        public bool Send(string message)
        {
            try
            {
                var body = Encoding.UTF8.GetBytes(message);

                _channel.BasicPublish(exchange: string.Empty,
                                     routingKey: ChannelName,
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
