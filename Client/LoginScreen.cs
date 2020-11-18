using System;
using System.Data;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Windows.Forms;
using static Client.ClientUtil;

namespace Client
{
    public partial class LoginScreen : Form
    {
        public LoginScreen()
        {
            InitializeComponent();
        }

        private async void loginButton_Click(object sender, EventArgs eventArgs)
        {
            TcpClient client;
            (string, int) ipAddressTuple;

            try
            {
                var (ip, port) = ipAddressTuple = ParseConnectionArgs(ipTextBox.Text, portTextBox.Text);
                client = new TcpClient(ip, port);

                await HandleLoginProcedure(client.GetStream(), usernameTextBox.Text, passwordTextBox.Text);
            }
            catch (ArgumentNullException)
            {
                messageLabel.Text = "None of the credential fields can be empty.";
                return;
            }
            catch (ArgumentException)
            {
                messageLabel.Text = "Provided server info is invalid.";
                return;
            }
            catch (SocketException)
            {
                messageLabel.Text = "Server went down, please contact the administrator.";
                return;
            }
            catch (InvalidCredentialException e)
            {
                messageLabel.Text = $"Invalid credentials: {e.Message}.";
                return;
            }
            catch (DuplicateNameException)
            {
                messageLabel.Text = "This account is already connected with the server right now.";
                return;
            }

            var mainScreen = new MainScreen(client, ipAddressTuple);

            Hide();
            mainScreen.Closed += (s, args) => Close();
            mainScreen.Show();
        }


        private void registerButton_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}