using System;
using System.IO;
using System.Windows.Forms;

namespace Client
{
    public partial class ChangePasswordScreen : Form
    {
        public ChangePasswordScreen()
        {
            InitializeComponent();
        }

        public (string, string, string) GetCredentials()
        {
            if (string.IsNullOrWhiteSpace(usernameTextBox.Text)
                || string.IsNullOrWhiteSpace(oldPasswordTextBox.Text)
                || string.IsNullOrWhiteSpace(newPasswordTextBox.Text))
            {
                throw new InvalidDataException("One of the fields was empty.");
            }

            return (usernameTextBox.Text, oldPasswordTextBox.Text, newPasswordTextBox.Text);
        }
    }
}