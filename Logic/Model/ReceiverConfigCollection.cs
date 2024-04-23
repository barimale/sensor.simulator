namespace Logic.Model
{
    public class ReceiverConfigCollection
    {
        public ReceiverConfigCollection()
        {
            Receivers = new List<ReceiverConfig>();
        }

        public List<ReceiverConfig> Receivers { get; set; }
    }
}
