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
        private ReceiverConfigCollection receivers;
        private TabControl tbdynamic = new TabControl();

        public TransmiterForm()
        {
            InitializeComponent();
        }

        private void TransmiterForm_Load(object sender, EventArgs e)
        {
            //preconfiguration
            Preconfigure();

            //configuration
            ReadSensors();
            ReadReceivers();

            // mapping
            MapSensorsToPages();
            MapSensorsToSimulators();
            MapSensorsToChannels();
        }

        private void MapSensorsToChannels()
        {
            var localhost = "localhost";
            // sensors to channels
            foreach (var item in receivers.Receivers.Where(p => p.IsActive))
            {
                var service = new PublishToChannelService(localhost);
                service.CreateChannel(item.ToChannelName());
                _channels.Add(service);
            }
        }

        private void MapSensorsToSimulators()
        {
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
        }

        private void MapSensorsToPages()
        {
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
        }

        private void ReadSensors()
        {
            try
            {
                // for each sensor in sensors
                var reader = new ConfigReader();
                var path = "e://sensorConfig.json";

                // when
                this.sensors = reader.ReadSensors(path);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void ReadReceivers()
        {
            try
            {
                // for each sensor in sensors
                var reader = new ConfigReader();
                var path = "e://receiverConfig.json";

                // when
                this.receivers = reader.ReadReceivers(path);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void Preconfigure()
        {
            this.Text = "Transmiter";
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            tbdynamic.Top = 30;
            tbdynamic.Height = this.Height - 30;
            tbdynamic.Width = this.Width;
        }

        private void Simulator_Tick(object sender, EventArgs e)
        {
            var tag = sender as System.Windows.Forms.Timer;
            if (tag == null)
                return;

            var tagID = (int)tag.Tag;

            var configurations = receivers
                .Receivers
                .Where(p => p.SensorId == tagID)
                .ToList();

            var cpunt = configurations.Count;

            foreach(var configuration in configurations)
            {
                if (configuration == null)
                    return;

                var channel = _channels
                    .FirstOrDefault(p => p.ChannelName == configuration.ToChannelName());
            
                if (channel == null)
                    return;

                var message = sensors
                    .Sensors
                    .FirstOrDefault(p => p.ID == configuration.SensorId)
                    .ToString();

                channel.Send(message);
            }
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
