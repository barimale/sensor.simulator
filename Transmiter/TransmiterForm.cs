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
                TabPage page = new TabPage();
                page.Text = item.ID.ToString();
                page.BackColor = Color.White;

                var label = new Label();
                label.AutoSize = true;
                label.Text = item.ToMultilineText();
                label.Text += "\n" + "Receivers: " + string.Join(
                    ',',
                    _transmitManager
                        .Receivers
                        .Receivers
                        .Where(pp => pp.SensorId == item.ID)
                        .Select(p => p.ID));
                label.Font = new Font("Arial", 14, FontStyle.Regular);
                label.ForeColor = Color.Black;
                page.Controls.Add(label);
                tbdynamic.TabPages.Add(page);
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
