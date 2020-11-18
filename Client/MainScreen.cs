using System;
using System.Data;
using System.Net.Sockets;
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

        private async void historyButton_Click(object sender, EventArgs e)
        {
            var results = await ProcessHistoryRequest(_client.GetStream());

            var historyScreen = new HistoryScreen(results);
            historyScreen.Show();

        }

        private async void logOutButton_Click(object sender, EventArgs e)
        {
            await ProcessDisconnectRequest(_client.GetStream());

            MessageBox.Show("You have been sucessfully logged out, the app will close now.", "Success");
            Close();
        }

    }
}
