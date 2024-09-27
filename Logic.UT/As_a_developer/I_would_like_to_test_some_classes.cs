using Logic.UT.BaseUT;
using Logic.Utilities;
using Xunit.Abstractions;

namespace Logic.UT.As_a_developer
{
    public class I_would_like_to_test_some_classes : PrintToConsoleUTBase
    {
        public I_would_like_to_test_some_classes(
            ITestOutputHelper output)
            : base(output)
        {
            // intentionally left blank
        }

        [Fact]
        public void Obtain_classification()
        {
            // given

            // when
            var result = ClassifierUtility.ClassifySignal();

            // then
            Assert.NotNull(result);
        }
    }
}
