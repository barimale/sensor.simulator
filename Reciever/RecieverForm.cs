using Logic.Model;
using Logic.Services;
using RabbitMQ.Client.Events;
using System.Text;

namespace Reciever
{
    public partial class RecieverForm : Form
    {
        private List<SubscribeToChannelService> _channels = new List<SubscribeToChannelService>();
        private SensorConfigCollection sensors;
        private ReceiverConfigCollection receivers;
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
            ReadReceivers();
            MapReceiversToPages();
            MapReceiversToChannels();
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
                var reader = new ConfigReader();
                var path = "e://receiverConfig.json";
                this.receivers = reader.ReadReceivers(path);
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

        private void MapReceiversToChannels()
        {
            var localhost = "localhost";
            // receivers to channels
            foreach (var item in receivers.Receivers.Where(p => p.IsActive))
            {
                var service = new SubscribeToChannelService(localhost);
                service.CreateChannel(item.ToChannelName());
                _channels.Add(service);
            }
        }

        private void MapReceiversToPages()
        {
            foreach (var item in receivers.Receivers)
            {
                TabPage mPage = new TabPage();
                mPage.Text = item.ToChannelName();
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
