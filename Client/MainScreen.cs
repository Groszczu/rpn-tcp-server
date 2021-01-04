using System;
using System.Linq;
using System.Net.Sockets;
using System.Windows.Forms;
using Client.Utility;
using RPN_Locale;
using static Client.Utility.Procedures;
using Application = Client.Model.Application;
using ArgumentException = System.ArgumentException;
using DataException = System.Data.DataException;

namespace Client
{
    public partial class MainScreen : Form
    {
        private readonly TcpClient _client;
        private readonly bool _isUserAdmin;

        public MainScreen(TcpClient client, string username)
        {
            InitializeComponent();

            _client = client;

            Text = $"RPN Calculator - Logged in as \"{username}\"";

            _isUserAdmin = (ProcessAdminRequest(_client.GetStream()) == CoreLocale.IsAdmin);
            requestAdminButton.Visible = !_isUserAdmin;
            adminRequestApprovalButton.Visible = bugReportsButton.Visible = _isUserAdmin;
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
                MessageBox.Show(
                    "Server didn't respond properly.\nAre you sure your expression is correct?\nIf the problem persists, contact an administrator.");
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

        private void requestAdminButton_Click(object sender, EventArgs e)
        {
            if (!_isUserAdmin)
            {
                var message = ProcessAdminRequest(_client.GetStream());

                MessageBox.Show(message, "Alert");
            }
            else
            {
                MessageBox.Show("Current user already has admin priviledges.", "Info");
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

        private async void bugReportsButton_Click(object sender, EventArgs e)
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

        private async void adminRequestApprovalButton_Click(object sender, EventArgs e)
        {
            try
            {
                var results = await ProcessInformationRequest(_client.GetStream(), Request.Applications);
                var applications = results.Select(Application.FromString).ToList();

                var adminScreen = new AdminApprovalScreen(_client, applications);
                adminScreen.Show();
            }
            catch (DataException)
            {
                MessageBox.Show("There are no pending admin applications.", "Info");
            }
        }
    }
}