using Logic.Model;

namespace Logic.Utilities
{
    public static class ClassifierUtility
    {
        private const int LOW_LEVEL = 0;
        private const int HIGH_LEVEL = 100;

        public static Classification ClassifySignal()
        {
            var randomizer = new Random();
            var result = randomizer.Next(LOW_LEVEL, HIGH_LEVEL + 1);

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
                case <= 100:
                    return Classification.Alarm;
                default:
                    return Classification.Alarm;
            }
        }
    }
}
