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
            // for each sensor in sensors
            // create tab in tabs
            // for each tab in tabs create channel rabbitmq
        }
    }
}
