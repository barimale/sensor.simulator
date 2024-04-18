using Logic.Services;

namespace Transmiter
{
    public partial class TransmiterForm : Form
    {
        public TransmiterForm()
        {
            InitializeComponent();
        }

        private void TransmiterForm_Load(object sender, EventArgs e)
        {
            this.Text = "Transmiter";

            TabControl tbdynamic = new TabControl();
            tbdynamic.Height = 200;
            tbdynamic.Width = 200;

            // for each sensor in sensors
            var reader = new ConfigReader();
            var path = "e://sensorConfig.json";

            // when
            var result = reader.Read(path);
            foreach ( var item in result.Sensors)
            {
                TabPage mPage = new TabPage();
                mPage.Text = "ID: " + item.ID;
                tbdynamic.TabPages.Add(mPage);
            }
           
           
            this.Controls.Add(tbdynamic);
            tbdynamic.BringToFront();
            // create tab in tabs
            // for each tab in tabs create channel rabbitmq
        }
    }
}
