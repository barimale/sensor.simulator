using Logic.Model;
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
        [InlineData("e:\\SensorConfig.json", 5)]
        public void Generate_sensor_config_file(string path, int sensorAmount)
        {
            // given
            var json = new SensorConfigCollection();
            // when
            for (int i = 1; i <= sensorAmount; i++)
            {
                var sensor = new SensorConfig()
                {
                    ID = i,
                    Type = "speed",
                    MinValue = -10,
                    MaxValue = 100,
                    EncoderType = "fixed",
                    Frequency = i,
                };

                json.Sensors.Add(sensor);
            }

            var content = JsonConvert.SerializeObject(json, Formatting.Indented);

            // then
            File.WriteAllText(path, content);
        }

        [Theory]
        [InlineData("e:\\ReceiverConfig.json", 5, 30)]
        public void Generate_receiver_config_file(string path, int sensorAmount, int receiverAmount)
        {
            // given
            var json = new ReceiverConfigCollection();
            var randomizer = new Random();
            // when
            for (int i = 1; i <= receiverAmount; i++)
            {
                var sensor = new ReceiverConfig()
                {
                    ID = i,
                    IsActive = true,
                    SensorId = randomizer.Next(1, sensorAmount)
                };

                json.Receivers.Add(sensor);
            }

            var content = JsonConvert.SerializeObject(json, Formatting.Indented);

            // then
            File.WriteAllText(path, content);
        }
    }
}
