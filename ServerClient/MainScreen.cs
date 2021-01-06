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
        private bool isRunned = false;

        public MainScreen()
        {
            InitializeComponent();
            Text = "Admin Panel";

            _ctrlWriter = new ControlWriter(logBox);
            roleButton.Visible = false;
            reportButton.Visible = false;
            stopButton.Visible = false;
        }

        private async void startButton_Click(object sender, EventArgs e)
        {
            try
            {
                roleButton.Visible = true;
                reportButton.Visible = true;
                stopButton.Visible = true;
                startButton.Visible = false;
                IPAddress ipAddress = null;
                var port = 0;

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

                var contextBuilder = new ContextBuilder(_ctrlWriter.Write);

                _rpnServer = new ResponseServerAsync(ipAddress, port, RPNCalculator.GetResult, Encoding.ASCII,
                    contextBuilder.CreateRpnContext, _ctrlWriter.Write);

                if (!isRunned)
                {
                    _rpnServer.Start();
                    isRunned = true;
                }
                else
                    _ctrlWriter.Write("[Alert] Server is started!");
            }
            catch (Exception)
            {
                _ctrlWriter.Write("[Alert] Start server to use implemented options");
            }
        }

        private async void stopButton_Click(object sender, EventArgs e)
        {
            startButton.Visible = true;
            roleButton.Visible = false;
            reportButton.Visible = false;
            stopButton.Visible = false;
            try
            {
                if (isRunned == true)
                {
                    await _rpnServer.Stop();
                    isRunned = false;
                }
                else
                    _ctrlWriter.Write("[Alert] Server is stopped");
            }
            catch (Exception)
            {
                _ctrlWriter.Write("[Alert] Object Disposed Exception");
            }
        }

        private void roleButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (_rpnServer.ApplicationRepository.UnresolvedAsObject().ToArray().Length == 0)
                    MessageBox.Show("[Alert] The privilege request list is empty", "Info");
                else
                {
                    var roleScreen = new RoleScreen("Give a role", _rpnServer);
                    roleScreen.Show();
                }

            }
            catch (DataException)
            {
                MessageBox.Show("[Alert] There are no users in data base", "Info");
                _ctrlWriter.Write("[Alert] There are no users in data base");
            }
        }

        private void reportButton_Click(object sender, EventArgs e)
        {
            try
            {
                var results = _rpnServer.ReportRepository.Reports.ToList();
                if (results.Count == 0)
                    MessageBox.Show("[Alert] The privilege request list is empty", "Info");
                else
                {
                    var reportScreen = new ReportScreen(results, "Bug reports");
                    reportScreen.Show();
                }

            }
            catch (DataException)
            {
                MessageBox.Show("[Alert] There are no bug reports", "Info");
            }
            catch (NullReferenceException)
            {
                _ctrlWriter.Write("[Alert] Start server to look into report list");
            }
        }
    }
}