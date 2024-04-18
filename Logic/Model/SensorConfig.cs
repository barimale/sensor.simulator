﻿using Logic.Utilities;

namespace Logic.Model
{
    public class SensorConfig
    {
        public int ID { get; set; }
        public string Type { get; set; }
        public int MinValue { get; set; }
        public int MaxValue { get; set; }
        public string EncoderType { get; set; }
        public int Frequency { get; set; }

        public string ToString()
        {
            var randomizer = new Random();
            var value = randomizer.Next(MinValue, MaxValue);

            var result = "$FIX," + this.ID.ToString() + "," + Type + "," + value + "," + Classifier.Classify();

            return result;
        }
    }
}
