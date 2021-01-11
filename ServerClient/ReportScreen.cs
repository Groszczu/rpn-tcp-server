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
    public partial class ReportScreen : Form
    {
        public ReportScreen(List<Report> elements, string title)
        {
            InitializeComponent();
            Text = $"Admin Panel - {title}";
            listBox.Items.AddRange(elements.ToArray());
        }
    }
}
