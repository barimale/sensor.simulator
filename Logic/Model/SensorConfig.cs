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

        public string ToTelegram()
        {
            var randomizer = new Random();
            var value = randomizer.Next(MinValue, MaxValue + 1);

            var result = "$FIX," + this.ID.ToString() + "," + Type + "," + value + "," + ClassifierUtility.ClassifySignal();

            return result;
        }

        public string ToMultilineText(string receivers)
        {
            var labelText = "ID: " + this.ID;
            labelText += "\n" + "Type: " + this.Type;
            labelText += "\n" + "MinValue: " + this.MinValue;
            labelText += "\n" + "MaxValue: " + this.MaxValue;
            labelText += "\n" + "EncoderType: " + this.EncoderType;
            labelText += "\n" + "Frequency: " + this.Frequency;
            labelText += "\n" + "Receivers: " + (string.IsNullOrEmpty(receivers) ? "-" : receivers);

            return labelText;
        }
    }
}
