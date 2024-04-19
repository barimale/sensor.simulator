using Logic.Model;
using Newtonsoft.Json;

namespace Logic.Services
{
    public class ConfigReader
    {
        public SensorConfigCollection? ReadSensors(string jsonFilePath)
        {
            return Read<SensorConfigCollection>(jsonFilePath);
        }

        public ReceiverConfigCollection? ReadReceivers(string jsonFilePath)
        {
            return Read<ReceiverConfigCollection>(jsonFilePath);
        }

        private T? Read<T>(string jsonFilePath)
        {
            try
            {
                string json = File.ReadAllText(jsonFilePath);
                var items = JsonConvert.DeserializeObject<T>(json);

                return items;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
