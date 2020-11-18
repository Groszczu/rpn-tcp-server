using System;
using System.Data;
using System.Drawing;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Client.ClientUtil;

namespace Client
{
    public partial class MainScreen : Form
    {

        private TcpClient _client;
        private readonly (string, int) _ipAddressTuple;

        public MainScreen(TcpClient client, (string, int) ipAddressTuple, string username)
        {
            InitializeComponent();

            _client = client;
            _ipAddressTuple = ipAddressTuple;

            Text = $"RPN Calculator - Logged in as \"{username}\"";
            if (username == "admin") reportViewButton.Visible = true;
        }

        private void rpnTextBox_TextChanged(object sender, EventArgs eventArgs) =>
            rpnTextBox.ValidateAndColorRpnExpression();

        private async void calculateButton_Click(object sender, EventArgs eventArgs)
        {
            try
            {
                rpnResultTextBox.Text = await ProcessCalculationRequest(_client.GetStream(), rpnTextBox.Text);
            }
            catch (ArgumentException)
            {
                MessageBox.Show("Your expression is not valid.", "Error");
            }
            catch (DataException)
            {
                MessageBox.Show("Server didn't respond properly. If the problem persists, contact an administrator.");
            }
        }

    }
}
