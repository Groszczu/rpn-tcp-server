using System;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;

namespace Client
{
    public partial class InitScreen : Form
    {
        private static readonly int MIN_PORT = 1024;
        private static readonly int MAX_PORT = 65535;

        public InitScreen()
        {
            InitializeComponent();
        }

        private void connectButton_Click(object sender, EventArgs eventArgs)
        {
            TcpClient client;

            try
            {
                var (ip, port) = ParseConnectionArgs(ipTextBox.Text, portTextBox.Text);
                client = new TcpClient(ip, port);

            }
            catch (Exception e)
            {
                infoLabel.Text = e.Message;
                return;
            }

            Hide();

            var loginScreen = new LoginScreen(client);

            loginScreen.Closed += (s, args) => Close();
            loginScreen.Show();
        }

        private static (string, int) ParseConnectionArgs(string ip, string port)
        {
            _ = IPAddress.Parse(ip);
            var portNumber = int.Parse(port);

            if (portNumber < MIN_PORT || portNumber > MAX_PORT)
            {
                throw new ArgumentException("invalid port number");
            }

            return (ip, portNumber);
        }
    }
}
