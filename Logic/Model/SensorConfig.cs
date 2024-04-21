using Logic.Utilities;
using static System.Net.Mime.MediaTypeNames;
using System.Drawing;
using System.Reflection.Emit;

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
            var value = randomizer.Next(MinValue, MaxValue);

            var result = "$FIX," + this.ID.ToString() + "," + Type + "," + value + "," + ClassifierUtilitycs.ClassifySignal();

            return result;
        }

        public string ToMultilineText()
        {
            var labelText = "ID: " + this.ID;
            labelText += "\n" + "Type: " + this.Type;
            labelText += "\n" + "MinValue: " + this.MinValue;
            labelText += "\n" + "MaxValue: " + this.MaxValue;
            labelText += "\n" + "EncoderType: " + this.EncoderType;
            labelText += "\n" + "Frequency: " + this.Frequency;

            return labelText;
        }
    }
}
