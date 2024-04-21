using Logic.Managers;
using Logic.Model;
using RabbitMQ.Client.Events;
using System.Text;

namespace Reciever
{
    public partial class RecieverForm : Form
    {
        private ConsumeManager _consumeManager;
        private TabControl tbdynamic = new TabControl();

        public RecieverForm()
        {
            InitializeComponent();
        }

        public string RabbitHostName { get; set; }
        public string SensorConfigPath { get; set; }
        public string ReceiverConfigPath { get; set; }

        private void Form1_Load(object sender, EventArgs e)
        {
            // preconfiguration
            Preconfigure();

            _consumeManager = new ConsumeManager(
                this.ReceiverConfigPath,
                this.SensorConfigPath,
                this.RabbitHostName);

            // mappings
            MapReceiversToPages();
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

        private void SubscribeChannels()
        {
            // subscribe to channels
            foreach (var channel in _consumeManager.Channels)
            {
                var consumer = new EventingBasicConsumer(channel.Channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);

                    var result = new SensorResult(message);
                    var sensor = _consumeManager.Sensors.Sensors.FirstOrDefault(p => p.ID == result.ID);
                    
                    ApplyChangesToUI(result, sensor);
                };

                channel.Consume(consumer);
            }
        }

        private void MapReceiversToPages()
        {
            foreach (var item in _consumeManager.Receivers.Receivers.Where(p => p.IsActive))
            {
                TabPage mPage = new TabPage();
                mPage.Text = item.ToChannelName();
                mPage.Tag = item.SensorId;
                mPage.BackColor = Color.White;
                tbdynamic.TabPages.Add(mPage);
            }

            this.Controls.Add(tbdynamic);
            tbdynamic.BringToFront();
        }

        private void ApplyChangesToUI(SensorResult result, SensorConfig? sensor)
        {
            if (result == null || result.Classification == null)
                return;

            foreach (Control page in tbdynamic.TabPages)
            {
                if ((int)page.Tag == sensor.ID)
                {
                    page.BackColor = result.FromClassificationToColor();

                    var json = new Label();
                    json.AutoSize = true;
                    json.Text = result.Value.ToString();
                    json.ForeColor = Color.Black;
                    json.Font = new Font("Arial", 24, FontStyle.Bold);

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
