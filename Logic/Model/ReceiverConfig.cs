namespace Logic.Model
{
    public class ReceiverConfig
    {
        public int ID { get; set; }
        public bool IsActive { get; set; }
        public int SensorId { get; set; }
        public string ToChannelName()
        {
            return $"{ID}.{SensorId}";
        }
    }
}
