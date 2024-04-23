using Logic.Model;
using Logic.Services;

namespace Logic.Managers
{
    public class ConsumeManager
    {
        private List<SubscribeToChannelService> _channels = new List<SubscribeToChannelService>();
        private SensorConfigCollection sensors;
        private ReceiverConfigCollection receivers;

        private ConsumeManager()
        {
            _channels = new List<SubscribeToChannelService>();
        }

        public ConsumeManager(string receiversPath, string sensorsPath, string hostName)
            : this()
        {
            ReadSensors(sensorsPath);
            ReadReceivers(receiversPath);
            // mapping
            MapReceiversToChannels(hostName);
        }

        public List<SensorConfig> Sensors => sensors.Sensors;
        public List<ReceiverConfig> Receivers => receivers.Receivers;
        public List<SubscribeToChannelService> Channels => _channels;

        private void MapReceiversToChannels(string hostName)
        {
            // receivers to channels
            foreach (var item in receivers.Receivers.Where(p => p.IsActive))
            {
                var service = new SubscribeToChannelService(hostName);
                service.CreateChannel(item.ToChannelName());
                _channels.Add(service);
            }
        }

        private void ReadSensors(string path)
        {
            try
            {
                var reader = new ConfigReader();
                this.sensors = reader.ReadSensors(path);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void ReadReceivers(string path)
        {
            try
            {
                var reader = new ConfigReader();
                this.receivers = reader.ReadReceivers(path);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
