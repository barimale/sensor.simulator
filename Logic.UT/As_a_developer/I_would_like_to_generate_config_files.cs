using Logic.Model;
using Logic.Services;
using Logic.UT.BaseUT;
using Newtonsoft.Json;
using Xunit.Abstractions;

namespace Logic.UT.As_a_developer
{
    public class I_would_like_to_generate_config_files : PrintToConsoleUTBase
    {
        public I_would_like_to_generate_config_files(
            ITestOutputHelper output)
            : base(output)
        {
            // intentionally left blank
        }

        [Theory]
        [InlineData("s:\\SensorConfig.json", 6)]
        public void Generate_sensor_config_file(string path, int sensorAmount)
        {
            // given
            var reader = new ConfigReader();
            var pathToSensors = ".//Data//sensorConfig.json";
            var sensors = reader.ReadSensors(pathToSensors);

            var json = new SensorConfigCollection();
            // when
            for (int i = 1; i <= sensorAmount; i++)
            {
                var template = sensors
                    .Sensors
                    .ToList()
                    .FirstOrDefault(p => p.ID == i);

                var sensor = new SensorConfig()
                {
                    ID = i,
                    Type = template.Type,
                    MinValue = template.MinValue,
                    MaxValue = template.MaxValue,
                    EncoderType = template.EncoderType,
                    Frequency = template.Frequency,
                };

                json.Sensors.Add(sensor);
            }

            var content = JsonConvert.SerializeObject(json, Formatting.Indented);

            // then
            File.WriteAllText(path, content);
        }

        [Theory]
        [InlineData("s:\\ReceiverConfig.json", 6, 30, 1)]
        [InlineData("s:\\ReceiverConfig2.json", 6, 30, 31)]
        public void Generate_receiver_config_file(
            string path,
            int sensorAmount,
            int receiverAmount,
            int receiverInitialID)
        {
            // given
            var json = new ReceiverConfigCollection();
            var randomizer = new Random();
            // when
            for (int i = receiverInitialID; i < receiverAmount + receiverInitialID; i++)
            {
                var sensor = new ReceiverConfig()
                {
                    ID = i,
                    IsActive = true,
                    SensorId = randomizer.Next(1, sensorAmount+1)
                };

                json.Receivers.Add(sensor);
            }

            var content = JsonConvert.SerializeObject(json, Formatting.Indented);

            // then
            File.WriteAllText(path, content);
        }
    }
}
