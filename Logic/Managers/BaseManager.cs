using Logic.Model;
using Logic.Services;

namespace Logic.Managers
{
    public abstract class BaseManager
    {
        private SensorConfigCollection sensors;
        private ReceiverConfigCollection receivers;

        public BaseManager(string receiversPath, string sensorsPath)
        {
            ReadSensors(sensorsPath);
            ReadReceivers(receiversPath);
        }

        public List<SensorConfig> Sensors => sensors.Sensors;
        public List<ReceiverConfig> Receivers => receivers.Receivers;
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

