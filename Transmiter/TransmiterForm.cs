using Logic.Managers;

namespace Transmiter
{
    public partial class TransmiterForm : Form
    {
        private TransmitManager _transmitManager;
        private TabControl tbdynamic = new TabControl();

        public TransmiterForm()
        {
            InitializeComponent();
        }

        public string RabbitHostName { get; set; }
        public string SensorConfigPath { get; set; }
        public string ReceiverConfigPath { get; set; }

        private void TransmiterForm_Load(object sender, EventArgs e)
        {
            //preconfiguration
            Preconfigure();

            //configuration
            _transmitManager = new TransmitManager(
                this.ReceiverConfigPath,
                this.SensorConfigPath,
                this.RabbitHostName);

            MapSensorsToPages();
        }

       
        private void MapSensorsToPages()
        {
            foreach (var item in _transmitManager.Sensors.Sensors)
            {
                TabPage mPage = new TabPage();
                mPage.Text = item.ID.ToString();
                mPage.BackColor = Color.White;

                var json = new Label();
                json.AutoSize = true;
                json.Text = "ID: " + item.ID;
                json.Text += "\n" + "Type: " + item.Type;
                json.Text += "\n" + "MinValue: " + item.MinValue;
                json.Text += "\n" + "MaxValue: " + item.MaxValue;
                json.Text += "\n" + "EncoderType: " + item.EncoderType;
                json.Text += "\n" + "Frequency: " + item.Frequency;
                json.ForeColor = Color.Black;
                mPage.Controls.Add(json);
                tbdynamic.TabPages.Add(mPage);
            }

            this.Controls.Add(tbdynamic);
            tbdynamic.BringToFront();
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

        private void sTARTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (var simulator in _transmitManager.Simulators)
            {
                simulator.Start();
            }
        }

        private void sTOPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (var simulator in _transmitManager.Simulators)
            {
                simulator.Stop();
            }
        }
    }
}
