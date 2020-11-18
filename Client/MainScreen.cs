using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public partial class MainScreen : Form
    {

        private TcpClient _client;

        private readonly NetworkStream _stream;
        private readonly StreamReader _streamReader;

        public MainScreen()
        {
            InitializeComponent();
        }

        public MainScreen(TcpClient client, NetworkStream stream, StreamReader streamReader)
        {
            _client = client;
            _stream = stream;
            _streamReader = streamReader;
        }
    }
}
