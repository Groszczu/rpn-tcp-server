using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Client.exceptions;

namespace Client
{
    public partial class LoginScreen : Form
    {
        private static readonly int BUFFER_SIZE = 1024;
        private static readonly int TASK_TIMEOUT = 500;

        private TcpClient _client;

        private readonly NetworkStream _stream;
        private readonly StreamReader _streamReader;

        public LoginScreen(TcpClient client)
        {
            InitializeComponent();

            _client = client;
            _stream = client.GetStream();
            _streamReader = new StreamReader(_stream);
        }

        private async void loginButton_Click(object sender, EventArgs eventArgs)
        {
            try
            {
                await HandleLoginButton();
            }
            catch (ServerDownException e)
            {
                titleLabel.Text = "Server went down, please contact the administrator.";
                return;
            }
            catch (InvalidCredentialException e)
            {
                titleLabel.Text = "Invalid credentials. Try again or create an account.";
                return;
            }

            Hide();

            var loginScreen = new MainScreen(_client, _stream, _streamReader);

            loginScreen.Closed += (s, args) => Close();
            loginScreen.Show();
        }

        private async Task HandleLoginButton()
        {
            var message = await ReadFromStreamAsync();
            if (message != "You are connected") throw new ServerDownException("server is not responding after a successful first connection");

            await FlushStreamAsync();
            await SendToStreamAsync(usernameTextBox.Text);

            await FlushStreamAsync();
            await SendToStreamAsync(passwordTextBox.Text);

            message = await ReadFromStreamAsync();
            if (message != "Enter RPN expression") throw new InvalidCredentialException("could not log in to the server");
        }

        private Task SendToStreamAsync(string message)
        {
            var messageLine = $"{message}\r\n";

            return _stream.WriteAsync(Encoding.UTF8.GetBytes(messageLine), 0, messageLine.Length);
        }

        private Task<string> ReadFromStreamAsync() => _streamReader.ReadLineAsync();

        private async Task<string> FlushStreamAsync()
        {
            using (var cancellationToken = new CancellationTokenSource())
            {
                string lastMessage = "";

                while (true)
                {
                    var task = _streamReader.ReadLineAsync();
                    var completedTask = await Task.WhenAny(task, Task.Delay(500, cancellationToken.Token));

                    if (completedTask == task)
                    {
                        cancellationToken.Cancel();
                        lastMessage = await task;
                    }

                    return lastMessage;
                }
            }
        }
    }
}