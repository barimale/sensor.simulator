using Logic.Model;
using Newtonsoft.Json;

namespace Logic.Services
{
    public class ConfigReader
    {
        public SensorConfigCollection? ReadSensors(string jsonFilePath)
        {
            try
            {
                string json = File.ReadAllText(jsonFilePath);
                var items = JsonConvert.DeserializeObject<SensorConfigCollection>(json);

                return items;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ReceiverConfigCollection? ReadReceivers(string jsonFilePath)
        {
            try
            {
                string json = File.ReadAllText(jsonFilePath);
                var items = JsonConvert.DeserializeObject<ReceiverConfigCollection>(json);

                return items;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
