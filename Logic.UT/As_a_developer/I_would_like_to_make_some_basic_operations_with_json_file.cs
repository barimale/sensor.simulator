using Logic.Services;
using Logic.UT.BaseUT;
using Logic.Utilities;
using Xunit.Abstractions;

namespace Logic.UT.As_a_developer
{
    public class I_would_like_to_make_some_basic_operations_with_json_file : PrintToConsoleUTBase
    {
        public I_would_like_to_make_some_basic_operations_with_json_file(
            ITestOutputHelper output)
            : base(output)
        {
            // intentionally left blank
        }

        [Fact]
        public void Read_sensors_config_file_and_map_to_class()
        {
            // given
            var reader = new ConfigReader();
            var path = ".//Data//sensorConfig.json";

            // when
            var result = reader.ReadSensors(path);

            // then
            Assert.NotNull(result);
            Assert.Equal(6, result.Sensors.Count);
        }

        [Fact]
        public void Read_receivers_config_file_and_map_to_class()
        {
            // given
            var reader = new ConfigReader();
            var path = ".//Data//receiverConfig.json";

            // when
            var result = reader.ReadReceivers(path);

            // then
            Assert.NotNull(result);
            Assert.Equal(11, result.Receivers.Count);
        }
    }
}
