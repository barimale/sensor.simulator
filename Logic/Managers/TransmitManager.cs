using Logic.Services;

namespace Logic.Managers
{
    public class TransmitManager : BaseManager
    {
        private List<PublishToChannelService> _channels = new List<PublishToChannelService>();
        private Dictionary<int, System.Timers.Timer> _simulators = new Dictionary<int, System.Timers.Timer>();

        public TransmitManager(string receiversPath, string sensorsPath, string hostName)
            : base(receiversPath, sensorsPath)
        {
            // mapping
            MapSensorsToSimulators();
            MapSensorsToChannels(hostName);
        }

        public List<System.Timers.Timer> Simulators => _simulators.Values.ToList();

        private void MapSensorsToChannels(string hostName)
        {
            // sensors to channels
            foreach (var item in Receivers.Where(p => p.IsActive))
            {
                var service = new PublishToChannelService(hostName);
                service.CreateChannel(item.ToChannelName());
                _channels.Add(service);
            }
        }
        private void MapSensorsToSimulators()
        {
            // sensors to simulators
            foreach (var item in Sensors)
            {
                var simulator = new System.Timers.Timer();
                simulator.AutoReset = true;
                double secondInMilliseconds = 1000d;
                simulator.Interval = (int)(secondInMilliseconds / item.Frequency);
                simulator.Elapsed += SimulatorTick;
                _simulators.Add(item.ID, simulator);
            }
        }

        private void SimulatorTick(object sender, EventArgs e)
        {
            var simulator = sender as System.Timers.Timer;
            if (simulator == null)
                return;

            var key = _simulators.Where(p => p.Value == simulator).FirstOrDefault().Key;
            var simulatorID = (int)key;

            var configurations = Receivers
                .Where(p => p.IsActive)
                .Where(p => p.SensorId == simulatorID)
                .ToList();

            var message = Sensors
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
    }
}
