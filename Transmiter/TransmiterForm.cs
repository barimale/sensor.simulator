using Logic.Managers;
using Logic.Model;

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
            foreach (var item in _transmitManager.Sensors)
            {
                TabPage page = new TabPage();
                page.Text = item.ID.ToString();
                page.BackColor = Color.White;

                var label = new Label();
                label.AutoSize = true;
                label.Text = item.ToMultilineText(GetLinkedReceivers(item));
                label.Font = new Font("Arial", 12, FontStyle.Regular);
                label.ForeColor = Color.Black;
                page.Controls.Add(label);
                tbdynamic.TabPages.Add(page);
            }

            this.Controls.Add(tbdynamic);
            tbdynamic.BringToFront();
        }

        private string GetLinkedReceivers(SensorConfig item)
        {
            return string.Join(
                ',',
                _transmitManager
                    .Receivers
                    .Where(pp => pp.SensorId == item.ID)
                    .Select(p => p.ID));
        }

        private void Preconfigure()
        {
            this.Text = "Transmiter";
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            tbdynamic.Top = 30;
            tbdynamic.Height = this.Height - 30;
            tbdynamic.Width = this.Width - 20;
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
