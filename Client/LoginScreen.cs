using System;
using System.Data;
using System.IO;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Threading.Tasks;
using System.Windows.Forms;
using Client.exceptions;

using static Client.ClientUtil;

namespace Client
{
    public partial class LoginScreen : Form
    {
        private TcpClient _client;
        private NetworkStream _stream;
        private StreamReader _streamReader;
        private (string, int) _ipAddressTuple;

        public LoginScreen()
        {
            InitializeComponent();
        }

        private async void loginButton_Click(object sender, EventArgs eventArgs)
        {
            try
            {
                var (ip, port) = _ipAddressTuple = ParseConnectionArgs(ipTextBox.Text, portTextBox.Text);
                _client = new TcpClient(ip, port);

                _stream = _client.GetStream();
                _streamReader = new StreamReader(_stream);

                await HandleLoginProcedure();
            }
            catch (ArgumentException e)
            {
                messageLabel.Text = e.Message;
                return;
            }
            catch (ServerDownException e)
            {
                loginLabel.Text = "Server went down, please contact the administrator.";
                return;
            }
            catch (InvalidCredentialException e)
            {
                loginLabel.Text = "Invalid credentials. Try again or create an account.";
                return;
            }
            catch (DuplicateNameException e)
            {
                loginLabel.Text = "This account is already connected with the server right now.";
                return;
            }

            Hide();

            var mainScreen = new MainScreen(_client, _stream, _streamReader);

            mainScreen.Closed += (s, args) => Close();
            mainScreen.Show();
        }

        /// <summary>
        /// Obsługuje naciśnięcie przycisku logowania.
        /// </summary>
        /// <exception cref="ServerDownException">Gdy serwer przestał odpowiadać.</exception>
        /// <exception cref="DuplicateNameException">Gdy użytkownik jest już zalogowany.</exception>
        /// <exception cref="ServerDownException">Gdy dane użytkownika nie istnieją w bazie danych serwera.</exception>
        private async Task HandleLoginProcedure()
        {
            var message = await ReadFromStreamAsync(_streamReader);
            if (message != "You are connected") throw new ServerDownException("server is not responding after a successful first connection");

            await SendToStreamAsync(_stream, usernameTextBox.Text);

            await FlushStreamAsync(_streamReader);

            await SendToStreamAsync(_stream, passwordTextBox.Text);

            message = await ReadFromStreamAsync(_streamReader);
            if (message == "User is already connected") throw new DuplicateNameException("a user with given username is already logged in");
            if (message == "User doesn't exist") throw new InvalidCredentialException("a user with given credentials does not exist");
        }
    }
}