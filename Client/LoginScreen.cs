using System;
using System.Data;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Threading.Tasks;
using System.Windows.Forms;
using Client.Utility;
using static Client.Utility.Core;
using static Client.Utility.Procedures;

namespace Client
{
    public partial class LoginScreen : Form
    {
        private TcpClient _client;

        public LoginScreen()
        {
            InitializeComponent();
        }

        private async void loginButton_Click(object sender, EventArgs eventArgs)
        {
            if (Cursor.Current == Cursors.WaitCursor) return;
            Cursor.Current = Cursors.WaitCursor;

            if (await IsProcedureSuccessful(AuthProcedure.Login))
            {
                SwitchToMainScreen();
            }

            Cursor.Current = Cursors.Default;
        }


        private async void registerButton_Click(object sender, EventArgs eventArgs)
        {
            if (Cursor.Current == Cursors.WaitCursor) return;
            Cursor.Current = Cursors.WaitCursor;

            if (await IsProcedureSuccessful(AuthProcedure.Register))
            {
                MessageBox.Show("An account has been created, you will be logged in.", "Success");
                SwitchToMainScreen();
            }

            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Wrapper funkcji przeprowadzającej autoryzację.
        /// Zwraca jej status po ukończeniu oraz wyświetla odpowiednie komunikaty w przypadku błędu.
        /// </summary>
        /// <param name="authProcedure">Rodzaj procedury</param>
        /// <returns>Prawdę jeśli procedura się powiodła.</returns>
        private async Task<bool> IsProcedureSuccessful(AuthProcedure authProcedure)
        {
            try
            {
                var (ip, port) = ParseConnectionArgs(ipTextBox.Text, portTextBox.Text);
                _client = new TcpClient(ip, port);

                await HandleAuthenticationProcedure(_client.GetStream(), authProcedure, usernameTextBox.Text,
                    passwordTextBox.Text);

                return true;
            }
            catch (ArgumentNullException)
            {
                MessageBox.Show("None of the credential fields can be empty.", "Error");
            }
            catch (ArgumentException)
            {
                MessageBox.Show("Provided server info is invalid.", "Error");
            }
            catch (SocketException)
            {
                MessageBox.Show("Server went down, please contact the administrator.", "Error");
            }
            catch (InvalidCredentialException e)
            {
                MessageBox.Show($"Invalid credentials: {e.Message}.", "Error");
            }
            catch (DuplicateNameException e)
            {
                MessageBox.Show($"Username error: {e.Message}", "Error");
            }

            return false;
        }

        /// <summary>
        /// Przełącza okno logowania na główne.
        /// </summary>
        private void SwitchToMainScreen()
        {
            var mainScreen = new MainScreen(_client, usernameTextBox.Text);

            Hide();
            mainScreen.Closed += (s, args) => Close();
            mainScreen.Show();
        }
    }
}