using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Windows.Forms;
using Application = Client.Model.Application;
using static Client.Utility.Core;

namespace Client
{
    public partial class AdminApprovalScreen : Form
    {
        private readonly TcpClient _client;

        public AdminApprovalScreen(TcpClient client, List<Application> applications)
        {
            InitializeComponent();

            _client = client;
            applicationListBox.Items.AddRange(applications.ToArray());
        }

        private async void acceptButton_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to accept selected applications?", "Warning",
                MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                foreach (Application item in applicationListBox.CheckedItems)
                {
                    await SendToStreamAsync(_client.GetStream(), item.AsAcceptMessage());
                }

                MessageBox.Show("Applications have been successfully accepted. This window will close now.", "Success");
            }
            else
            {
                MessageBox.Show("No changes were made. This window will close now.", "Info");
            }

            Close();
        }

        private async void declineButton_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to decline selected applications?", "Warning",
                MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                foreach (Application item in applicationListBox.CheckedItems)
                {
                    await SendToStreamAsync(_client.GetStream(), item.AsDeclineMessage());
                }

                MessageBox.Show("Applications have been successfully declined. This window will close now.", "Success");
            }
            else
            {
                MessageBox.Show("No changes were made. This window will close now.", "Info");
            }

            Close();
        }
    }
}