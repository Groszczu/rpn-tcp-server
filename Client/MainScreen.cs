using System;
using System.Net.Sockets;
using System.Windows.Forms;
using Client.Utility;
using static Client.Utility.Procedures;
using ArgumentException = System.ArgumentException;
using DataException = System.Data.DataException;

namespace Client
{
    public partial class MainScreen : Form
    {
        private readonly TcpClient _client;

        public MainScreen(TcpClient client, string username)
        {
            InitializeComponent();

            _client = client;

            Text = $"RPN Calculator - Logged in as \"{username}\"";
            reportViewButton.Visible = (username == "admin");
        }

        private void rpnTextBox_TextChanged(object sender, EventArgs eventArgs) => rpnTextBox.HighlightRpnExpression();

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
                MessageBox.Show("Server didn't respond properly.\nAre you sure your expression is correct?\nIf the problem persists, contact an administrator.");
            }
        }

        private async void historyButton_Click(object sender, EventArgs e)
        {
            try
            {
                var results = await ProcessInformationRequest(_client.GetStream(), Request.History);

                var historyScreen = new ListScreen(results, "Calculation history");
                historyScreen.Show();
            }
            catch (DataException)
            {
                MessageBox.Show("You haven't made any calculations yet.", "Info");
            }

        }

        private async void reportViewButton_Click(object sender, EventArgs e)
        {
            try
            {
                var results = await ProcessInformationRequest(_client.GetStream(), Request.Reports);

                var reportScreen = new ListScreen(results, "Bug reports");
                reportScreen.Show();
            }
            catch (DataException)
            {
                MessageBox.Show("There are no bug reports.", "Info");
            }
        }

        private async void logOutButton_Click(object sender, EventArgs e)
        {
            await ProcessDisconnectRequest(_client.GetStream());

            MessageBox.Show("You have been sucessfully logged out, the app will close now.", "Success");
            Close();
        }

        private async void sendReportButton_Click(object sender, EventArgs e)
        {
            try
            {
                await ProcessBugReportRequest(_client.GetStream(), reportTextBox.Text);

                MessageBox.Show("Your report has been submitted. Thank you for your feedback.", "Success");

                reportTextBox.Text = string.Empty;
            }
            catch (ArgumentException)
            {
                MessageBox.Show("Bug report cannot be empty", "Error");
            }
        }

    }
}
