using System.Drawing;

namespace Logic.Model
{
    public class ReceivedTelegram
    {
        public ReceivedTelegram(string fromMessage)
        {
            var items = fromMessage.Split(',').ToList();
            ID = int.Parse(items[1]);
            Type = items[2];
            Value = int.Parse(items[3]);
            Classification = Enum.Parse<Classification>(items[4]);
        }

        public int ID { get; set; }
        public string Type { get; set; }
        public int Value { get; set; }
        public Classification Classification { get; set; }

        public Color FromClassificationToColor()
        {
            switch (Classification)
            {
                case Classification.Normal:
                    return Color.White;
                case Classification.Warning:
                    return Color.Yellow;
                case Classification.Alarm:
                    return Color.Red;
                default:
                    return Color.White;
            }
        }
    }
}
