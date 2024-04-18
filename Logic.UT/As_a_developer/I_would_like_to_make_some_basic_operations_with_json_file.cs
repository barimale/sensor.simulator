using Logic.UT.BaseUT;
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
        public void Obtain_classification()
        {
            // given
            var classifier = new Classifier();
            var path = ".//Data//sensorConfig.json";

            // when
            var result = classifier.Classify();

            // then
            Assert.NotNull(result);
        }
    }
}
