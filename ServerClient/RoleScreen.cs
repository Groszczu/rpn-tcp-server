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
        private List<AdminApplication> _elements;
        public RoleScreen(List<AdminApplication> elements, string title)
        {
            InitializeComponent();
            Text = $"Admin Panel - {title}";
            _elements = elements;
            userBox.Items.AddRange(elements.ToArray());
            userBox.CheckOnClick = true;
        }

        private void acceptButton_Click(object sender, EventArgs e)
        {
            foreach (AdminApplication elem in _elements)
            {
                foreach (AdminApplication item in userBox.SelectedItems)
                {
                    if (item.UserId == elem.UserId)
                    {
                        _elements.Remove(new AdminApplication { UserId = item.UserId});
                    }
                }
            }
            userBox.Items.Clear();
            userBox.Items.AddRange(_elements.ToArray());
        }

        private void rejectButton_Click(object sender, EventArgs e)
        {
            foreach (AdminApplication elem in _elements)
            {
                foreach (AdminApplication item in userBox.SelectedItems)
                {
                    if (item.UserId == elem.UserId)
                    {
                        _elements.Remove(new AdminApplication { UserId = item.UserId });
                        elem.IsRejected = true;
                    }
                }
            }
            userBox.Items.Clear();
            userBox.Items.AddRange(_elements.ToArray());
        }
    }
}
