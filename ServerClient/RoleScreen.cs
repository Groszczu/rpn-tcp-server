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
        public RoleScreen(List<User> elements, string title)
        {
            InitializeComponent();
            Text = $"Admin Panel - {title}";
            var list = new List<string>();
            foreach (var x in elements)
                list.Add(x.Username);

            userBox.Items.AddRange(list.ToArray());
            userBox.CheckOnClick = true;
        }

        private void acceptButton_Click(object sender, EventArgs e)
        {

        }

        private void rejectButton_Click(object sender, EventArgs e)
        {

        }
    }
}
