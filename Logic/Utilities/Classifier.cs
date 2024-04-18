namespace Logic.Utilities
{
    public static class Classifier
    {
        public static Classification Classify()
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
                default:
                    return Classification.Alarm;
            }
        }
    }

    public enum Classification
    {
        Alarm,
        Warning,
        Normal
    }
}
