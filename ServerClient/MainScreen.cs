using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using RPN_Calculator;
using RPN_Database;
using RPN_TcpServer;
using static ServerClient.Functions.LogService;

namespace ServerClient
{
    public partial class MainScreen : Form
    {
        private ResponseServerAsync _rpnServer;
        private ControlWriter _ctrlWriter;
        public MainScreen()
        {
            InitializeComponent();
            Text = "Admin Panel";

            _ctrlWriter = new ControlWriter(logBox);
        }

        private async void startButton_Click(object sender, EventArgs e)
        {
            IPAddress ipAddress = null;
            var port = 0;

            Console.WriteLine(ipBox.TextLength);
            Console.WriteLine(portBox.TextLength);
            if (ipBox.TextLength.Equals(0) && portBox.TextLength.Equals(0))
            {
                ipAddress = IPAddress.Loopback;
                port = 1024;
            }
            else
            {
                ipAddress = IPAddress.Parse(ipBox.Text);
                port = int.Parse(portBox.Text);
            }
            
            if (ipAddress == null || port == 0)
            {
                Console.WriteLine("Please enter following arguments: 1. IP address, 2. port");
                return;
            }

            Console.WriteLine(ipAddress.ToString());

            _rpnServer = new ResponseServerAsync(ipAddress, port, RPNCalculator.GetResult, Encoding.ASCII, ContextBuilder.CreateRpnContext, _ctrlWriter.Write);
            _rpnServer.Start();
        }

        private async void stopButton_Click(object sender, EventArgs e)
        {
            await _rpnServer.Stop();
        }

        private void roleButton_Click(object sender, EventArgs e)
        {
            try
            {
                var results = _rpnServer.UserRepository.Users.ToList();
                var reportScreen = new RoleScreen(results, "Give a role");
                reportScreen.Show();
            }
            catch (DataException)
            {
                MessageBox.Show("There are no bug reports.", "Info");
            }
        }

        private void reportButton_Click(object sender, EventArgs e)
        {
           try
            {
                var results = new List<String> { "1", "2" };
                var reportScreen = new ReportScreen(results, "Bug reports");
                reportScreen.Show();
            }
            catch (DataException)
            {
                MessageBox.Show("There are no bug reports.", "Info");
            }
        }
    }
}
