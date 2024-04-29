using Logic.Services;

namespace Logic.Managers
{
    public class ConsumeManager: BaseManager
    {
        private List<SubscribeToChannelService> _channels = new List<SubscribeToChannelService>();

        public ConsumeManager(string receiversPath, string sensorsPath, string hostName)
            : base(receiversPath, sensorsPath)
        {
            MapReceiversToChannels(hostName);
        }
        public List<SubscribeToChannelService> Channels => _channels;

        private void MapReceiversToChannels(string hostName)
        {
            // receivers to channels
            foreach (var item in Receivers.Where(p => p.IsActive))
            {
                var service = new SubscribeToChannelService(hostName);
                service.CreateChannel(item.ToChannelName());
                _channels.Add(service);
            }
        }
    }
}
