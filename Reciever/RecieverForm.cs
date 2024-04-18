using Logic.Model;
using Logic.Services;
using Newtonsoft.Json;
using RabbitMQ.Client.Events;
using System.Text;

namespace Reciever
{
    public partial class RecieverForm : Form
    {
        private List<SubscribeToChannelService> _channels = new List<SubscribeToChannelService>();
        private SensorConfigCollection sensors;
        private TabControl tbdynamic = new TabControl();

        public RecieverForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // preconfiguration
            Preconfigure();

            // configuration
            ReadSensors();
            MapSensorsToPages();
            MapSensorsToChannels();
            SubscribeChannels();
        }

        private void Preconfigure()
        {
            this.Text = "Reciever";
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            tbdynamic.Top = 0;
            tbdynamic.Height = this.Height;
            tbdynamic.Width = this.Width;
        }

        private void ReadSensors()
        {
            try
            {
                var reader = new ConfigReader();
                var path = "e://sensorConfig.json";
                this.sensors = reader.Read(path);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void SubscribeChannels()
        {
            // subscribe to channels
            foreach (var channel in _channels)
            {
                // get channel 
                var consumer = new EventingBasicConsumer(channel.Channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);

                    var result = new SensorResult(message);
                    var sensor = sensors.Sensors.FirstOrDefault(p => p.ID == result.ID);
                    
                    ApplyChanges(result, sensor);
                };

                channel.Consume(consumer);
            }
        }

        private void MapSensorsToChannels()
        {
            var localhost = "localhost";
            // sensors to channels
            foreach (var item in sensors.Sensors)
            {
                var service = new SubscribeToChannelService(localhost);
                service.CreateChannel(item.ID.ToString());
                _channels.Add(service);
            }
        }

        private void MapSensorsToPages()
        {
            foreach (var item in sensors.Sensors)
            {
                TabPage mPage = new TabPage();
                mPage.Text = "ID:" + item.ID;
                mPage.Tag = item.ID;
                mPage.BackColor = Color.White;
                tbdynamic.TabPages.Add(mPage);
            }

            this.Controls.Add(tbdynamic);
            tbdynamic.BringToFront();
        }

        private void ApplyChanges(SensorResult result, SensorConfig? sensor)
        {
            foreach (Control page in tbdynamic.TabPages)
            {
                if ((int)page.Tag == sensor.ID)
                {
                    page.BackColor = result.FromClassificationToColor();

                    var json = new Label();
                    json.AutoSize = true;
                    json.Text = result.Value.ToString();
                    json.ForeColor = Color.Black;
                    try
                    {
                        this.Invoke(
                          new Action(() =>
                          {
                              page.Controls.Clear();
                              page.Controls.Add(json);
                          }));
                    }
                    catch (Exception)
                    {
                        // intentionally left blank
                    }

                }
            }
        }
    }
}
