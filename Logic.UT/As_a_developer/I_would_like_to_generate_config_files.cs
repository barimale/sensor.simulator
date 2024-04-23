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
        [InlineData("e:\\SensorConfig.json", 50)]
        public void Generate_config_file(string path, int sensorAmount)
        {
            // given
            var json = new SensorConfigCollection();
            // when
            for(int i = 1; i <= sensorAmount; i++)
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

            var content = JsonConvert.SerializeObject(json);

            // then
            File.WriteAllText(path, content);

        }
    }
}
