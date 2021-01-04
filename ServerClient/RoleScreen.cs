using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RPN_Database.Model;

namespace ServerClient
{
    public partial class RoleScreen : Form
    {
        public RoleScreen(List<AdminApplication> elements, string title)
        {
            InitializeComponent();
            Text = $"Admin Panel - {title}";

            userBox.Items.AddRange(elements.ToArray());
            //userBox.CheckOnClick = true;
        }

        private void acceptButton_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to decline selected applications?", "Warning",
                MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                foreach (Application item in userBox.CheckedItems)
                {
                    await SendToStreamAsync(_client.GetStream(), item.AsAcceptMessage());
                }

                MessageBox.Show("Applications have been successfully accepted. This window will close now.", "Success");
            }
            else
            {
                MessageBox.Show("No changes were made. This window will close now.", "Info");
            }

        }

        private void rejectButton_Click(object sender, EventArgs e)
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

        }
    }
}
