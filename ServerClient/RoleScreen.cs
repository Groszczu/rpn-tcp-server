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
using RPN_TcpServer;

namespace ServerClient
{
    public partial class RoleScreen : Form
    {
        private ResponseServerAsync _rpnServer;

        public RoleScreen(string title, ResponseServerAsync rpn)
        {
            InitializeComponent();
            Text = $"Admin Panel - {title}";
            _rpnServer = rpn;
            var unresolvArray = _rpnServer.ApplicationRepository.UnresolvedAsObject().ToArray();
            if (unresolvArray.Length == 0)
            {
                userBox.Items.AddRange(new string[] {});
            }
            else
                userBox.Items.AddRange(unresolvArray);
            userBox.CheckOnClick = true;
        }

        private void acceptButton_Click(object sender, EventArgs e)
        {
            foreach (AdminApplication item in userBox.SelectedItems)
            {
                _rpnServer.ApplicationRepository.UpdateRejection(item.Id, false);
                MessageBox.Show("[Alert] Admin privileges successfully granted. Open the window again to continue.", "Info");
            }
            Close();
        }

        private void rejectButton_Click(object sender, EventArgs e)
        {
            foreach (AdminApplication item in userBox.SelectedItems)
            {
                _rpnServer.ApplicationRepository.UpdateRejection(item.Id, true);
                MessageBox.Show("[Alert] The rejection of the privileges was successful. Open the window again to continue.", "Info");
            }
            Close();
        }
    }
}
