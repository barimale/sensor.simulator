using Logic.Model;
using Logic.Services;

namespace Logic.Managers
{
    public class TransmitManager
    {
        private List<PublishToChannelService> _channels = new List<PublishToChannelService>();
        private Dictionary<int, System.Timers.Timer> _simulators = new Dictionary<int, System.Timers.Timer>();
        private SensorConfigCollection sensors;
        private ReceiverConfigCollection receivers;

        private TransmitManager()
        {
            _channels = new List<PublishToChannelService>();
            _simulators = new Dictionary<int, System.Timers.Timer>();
        }
        public TransmitManager(string receiversPath, string sensorsPath, string hostName)
            : this()
        {
            ReadSensors(sensorsPath);
            ReadReceivers(receiversPath);
            // mapping
            MapSensorsToSimulators();
            MapSensorsToChannels(hostName);
        }

        public List<SensorConfig> Sensors => sensors.Sensors;
        public List<ReceiverConfig> Receivers => receivers.Receivers;
        public List<System.Timers.Timer> Simulators => _simulators.Values.ToList();

        private void MapSensorsToChannels(string hostName)
        {
            // sensors to channels
            foreach (var item in receivers.Receivers.Where(p => p.IsActive))
            {
                var service = new PublishToChannelService(hostName);
                service.CreateChannel(item.ToChannelName());
                _channels.Add(service);
            }
        }
        private void MapSensorsToSimulators()
        {
            // sensors to simulators
            foreach (var item in sensors.Sensors)
            {
                var simulator = new System.Timers.Timer();
                simulator.AutoReset = true;
                double secondInMilliseconds = 1000;
                simulator.Interval = (int)(secondInMilliseconds / item.Frequency);
                simulator.Elapsed += Simulator_Tick;
                _simulators.Add(item.ID, simulator);
            }
        }

        private void Simulator_Tick(object sender, EventArgs e)
        {
            var simulator = sender as System.Timers.Timer;
            if (simulator == null)
                return;

            var key = _simulators.Where(p => p.Value == simulator).FirstOrDefault().Key;
            var simulatorID = (int)key;

            var configurations = receivers
                .Receivers
                .Where(p => p.IsActive)
                .Where(p => p.SensorId == simulatorID)
                .ToList();

            var message = sensors
                   .Sensors
                   .FirstOrDefault(p => p.ID == simulatorID)
                   .ToTelegram();

            foreach (var configuration in configurations)
            {
                if (configuration == null)
                    continue;

                var channel = _channels
                    .FirstOrDefault(p => p.ChannelName == configuration.ToChannelName());

                if (channel == null)
                    continue;

                channel.Send(message);
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
