using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public partial class HistoryScreen : Form
    {
        public HistoryScreen(List<string> elements)
        {
            InitializeComponent();

            listBox.Items.AddRange(elements.ToArray());
        }
    }
}
