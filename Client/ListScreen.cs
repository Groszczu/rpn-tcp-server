using System.Collections.Generic;
using System.Windows.Forms;

namespace Client
{
    public partial class ListScreen : Form
    {
        public ListScreen(List<string> elements, string title)
        {
            InitializeComponent();

            Text = $"RPN Calculator - {title}";
            listBox.Items.AddRange(elements.ToArray());
        }
    }
}
