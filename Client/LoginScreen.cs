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
                MessageBox.Show("None of the credential fields can be empty.", "Error");
                return;
            }
            catch (ArgumentException)
            {
                MessageBox.Show("Provided server info is invalid.", "Error");
                return;
            }
            catch (SocketException)
            {
                MessageBox.Show("Server went down, please contact the administrator.", "Error");
                return;
            }
            catch (InvalidCredentialException e)
            {
                MessageBox.Show($"Invalid credentials: {e.Message}.", "Error");
                return;
            }
            catch (DuplicateNameException)
            {
                MessageBox.Show("This account is already connected with the server right now.", "Error");
                return;
            }

            var mainScreen = new MainScreen(client, ipAddressTuple, usernameTextBox.Text);

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