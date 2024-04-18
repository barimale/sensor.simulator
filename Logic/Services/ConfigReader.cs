using Logic.Model;
using Newtonsoft.Json;

namespace Logic.Services
{
    public class ConfigReader
    {
        public SensorConfigCollection? Read(string jsonFilePath)
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
       
    }
}
