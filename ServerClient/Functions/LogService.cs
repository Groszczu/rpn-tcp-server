using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ServerClient.Functions
{
    class LogService
    {
        public static void SetVisibility(List<Button> buttons, bool value)
        {
            foreach (var elem in buttons)
                elem.Visible = value;
        }
        public class ControlWriter : TextWriter
        {
            private Control textbox;


            public ControlWriter(Control textbox)
            {
                this.textbox = textbox;
            }

            public override void Write(string value)
            {
                try
                {
                    textbox.Text += value + "\n";
                }
                catch (InvalidOperationException)
                {
                    Console.WriteLine("Exception in Control Writer");
                }
            }

            public override Encoding Encoding
            {
                get { return Encoding.ASCII; }
            }
        }
    }
}
