namespace Logic.Utilities
{
    public static class ClassifierUtilitycs
    {
        public static Classification ClassifySignal()
        {
            var randomizer = new Random();
            var result = randomizer.Next(0, 100);

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

    public enum Classification
    {
        Alarm = 0,
        Warning,
        Normal
    }
}
