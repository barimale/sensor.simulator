using Logic.Model;
using Logic.Services;
using Newtonsoft.Json;

namespace Transmiter
{
    public partial class TransmiterForm : Form
    {
        private List<PublishToChannelService> _channels = new List<PublishToChannelService>();
        private List<System.Windows.Forms.Timer> simulators = new List<System.Windows.Forms.Timer>();
        private SensorConfigCollection sensors;
        public TransmiterForm()
        {
            InitializeComponent();
        }

        private void TransmiterForm_Load(object sender, EventArgs e)
        {
            this.Text = "Transmiter";

            TabControl tbdynamic = new TabControl();
            tbdynamic.Top = 100;
            tbdynamic.Height = 200;
            tbdynamic.Width = 800;

            // for each sensor in sensors
            var reader = new ConfigReader();
            var path = "e://sensorConfig.json";

            // when
            this.sensors = reader.Read(path);

            // sensors to tabPages
            foreach (var item in sensors.Sensors)
            {
                TabPage mPage = new TabPage();
                mPage.Text = "ID:" + item.ID;
                mPage.BackColor = Color.White;

                var json = new Label();
                json.AutoSize = true;
                json.Text = JsonConvert.SerializeObject(item);
                json.ForeColor = Color.Black;
                mPage.Controls.Add(json);
                tbdynamic.TabPages.Add(mPage);
            }

            this.Controls.Add(tbdynamic);
            tbdynamic.BringToFront();

            // sensors to simulators
            foreach (var item in sensors.Sensors)
            {
                var simulator = new System.Windows.Forms.Timer();
                double number = 1000;
                simulator.Interval = (int)(number / item.Frequency); // seconds
                simulator.Tick += new EventHandler(Simulator_Tick);
                simulator.Tag = item.ID;
                simulators.Add(simulator);
            }

            var localhost = "localhost";
            // sensors to channels
            foreach (var item in sensors.Sensors)
            {
                var service = new PublishToChannelService(localhost);
                service.CreateChannel(item.ID.ToString());
                _channels.Add(service);
            }
        }

        private void Simulator_Tick(object sender, EventArgs e)
        {
            var tag = sender as System.Windows.Forms.Timer;
            if (tag == null)
                return;

            var tagID = (int)tag.Tag;

            var configuration = sensors
                .Sensors
                .FirstOrDefault(p => p.ID == tagID);

            if (configuration == null)
                return;

            var channel = _channels
                .FirstOrDefault(p => p.ChannelName == configuration.ID.ToString());
            
            if (channel == null)
                return;

            var message = configuration.ToString();
            channel.Send(message);
        }

        private void sTARTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (var simulator in simulators)
            {
                simulator.Start();
            }
        }

        private void sTOPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (var simulator in simulators)
            {
                simulator.Stop();
            }
        }
    }
}
