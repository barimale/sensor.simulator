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
            this.Text = "Reciever";

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
                mPage.Tag = item.ID;
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


            var localhost = "localhost";
            // sensors to channels
            foreach (var item in sensors.Sensors)
            {
                var service = new SubscribeToChannelService(localhost);
                service.CreateChannel(item.ID.ToString());
                _channels.Add(service);
            }

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

                    // WIP add to label in tabPage
                    foreach(Control page in tbdynamic.TabPages)
                    {
                        if((int)page.Tag == sensor.ID)
                        {
                            page.BackColor = result.FromClassificationToColor();
                            // apply backcolor
                            //page.Text = result.Value.ToString();
                            // WIP apply value
                            var json = new Label();
                            json.AutoSize = true;
                            json.Text = JsonConvert.SerializeObject(result);
                            json.ForeColor = Color.Black;
                            //page.Controls.Clear();
                            //page.Controls.Add(json);
                        }
                    }
                    Console.WriteLine($" [x] Received {message}");
                };

                channel.Consume(consumer);
            }
        }
    }
}
