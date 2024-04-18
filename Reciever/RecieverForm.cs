using Logic.Model;

namespace Reciever
{
    public partial class RecieverForm : Form
    {
        private SensorConfigCollection sensors;

        public RecieverForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = "Reciever";
        }
    }
}
