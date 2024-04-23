namespace Logic.Model
{
    public class SensorConfigCollection
    {
        public SensorConfigCollection()
        {
            Sensors = new List<SensorConfig>();
        }

        public List<SensorConfig> Sensors { get; set; }
    }
}
