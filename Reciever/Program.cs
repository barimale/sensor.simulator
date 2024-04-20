namespace Reciever
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            ApplicationConfiguration.Initialize();

            var form = new RecieverForm();

            if (args.Length == 3)
            {
                form.RabbitHostName = args[0];
                form.ReceiverConfigPath = args[1];
                form.SensorConfigPath = args[2];
            }

            Application.Run(form);
        }
    }
}