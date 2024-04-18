using Xunit.Abstractions;

namespace Logic.UT.BaseUT
{
    public abstract class PrintToConsoleUTBase
    {
        private readonly ITestOutputHelper Output;

        public PrintToConsoleUTBase(ITestOutputHelper output)
        {
            Output = output;
        }

        public void Display(string line)
        {
            Output.WriteLine(line);
        }

        public void Display(string[] lines)
        {
            foreach (var line in lines)
            {
                Display(line);
            }
        }
    }
}
