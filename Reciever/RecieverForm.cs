using Logic.Managers;
using Logic.Model;
using RabbitMQ.Client.Events;
using System.Text;

namespace Reciever
{
    public partial class RecieverForm : Form
    {
        private ConsumeManager _consumeManager;
        private FlowLayoutPanel tbdynamic = new FlowLayoutPanel();

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
            MapReceiversToGroupBoxes();
            SubscribeToChannels();
        }

        private void Preconfigure()
        {
            this.Text = "Reciever";
            tbdynamic.Top = 0;
            tbdynamic.Height = this.Height;
            tbdynamic.Width = this.Width;
            tbdynamic.FlowDirection = FlowDirection.LeftToRight;
            tbdynamic.Margin = new Padding(50);
            tbdynamic.Dock = DockStyle.Fill;
            tbdynamic.AutoScroll = true;
            tbdynamic.WrapContents = true;
        }

        private void SubscribeToChannels()
        {
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

        private void MapReceiversToGroupBoxes()
        {
            foreach (var item in _consumeManager.Receivers.Receivers.Where(p => p.IsActive))
            {
                GroupBox groupBox = new GroupBox();
                groupBox.Text = item.ToChannelName();
                groupBox.Tag = item.ToChannelName();
                groupBox.AutoSizeMode = AutoSizeMode.GrowAndShrink;

                Label label = new Label();
                label.Tag = item.SensorId;
                label.BackColor = Color.Transparent;
                label.Height *= 4;
                label.Width *= 3;
                label.Padding = new Padding(30);
                label.Dock = DockStyle.Fill;
                groupBox.Controls.Add(label);

                tbdynamic.Controls.Add(groupBox);
            }

            this.Controls.Add(tbdynamic);
            tbdynamic.BringToFront();
        }

        private void ApplyChangesToUI(SensorResult result, SensorConfig? sensor)
        {
            if (result == null || result.Classification == null)
                return;

            foreach (Control page in tbdynamic.Controls)
            {
                foreach (Control control in page.Controls)
                {
                    if ((int)control.Tag == sensor.ID)
                    {

                        try
                        {
                            control.BackColor = result.FromClassificationToColor();

                            var label = new Label();
                            label.AutoSize = true;
                            label.Text = result.Value.ToString();
                            label.ForeColor = Color.Black;
                            label.Font = new Font("Arial", 24, FontStyle.Bold);

                            this.Invoke(
                              new Action(() =>
                              {
                                  control.Controls.Clear();
                                  control.Controls.Add(label);
                              }));
                        }
                        catch (Exception)
                        {
                            // intentionally left blank
                        }
                        finally{
                            GC.Collect();
                            GC.WaitForPendingFinalizers(); // maybe not 
                        }
                    }
                }
            }
        }
    }
}
