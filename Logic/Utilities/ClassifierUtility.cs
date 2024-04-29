using Logic.Model;

namespace Logic.Utilities
{
    public static class ClassifierUtility
    {
        private const int MIN_VALUE = 0;
        private const int MAX_VALUE = 100;

        public static Classification ClassifySignal()
        {
            var randomizer = new Random();
            var result = randomizer.Next(MIN_VALUE, MAX_VALUE);

            switch (result)
            {
                case < 10:
                    return Classification.Alarm;
                case < 25:
                    return Classification.Warning;
                case < 75:
                    return Classification.Normal;
                case < 90:
                    return Classification.Warning;
                case < 100:
                    return Classification.Alarm;
                default:
                    return Classification.Alarm;
            }
        }
    }
}
