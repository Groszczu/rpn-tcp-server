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
            userBox.Items.AddRange(elements.ToArray());
            userBox.CheckOnClick = true;
        }
    }
}
