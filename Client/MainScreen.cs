using System.Net.Sockets;
using System.Windows.Forms;

namespace Client
{
    public partial class MainScreen : Form
    {

        private TcpClient _client;
        private readonly (string, int) _ipAddressTuple;

        public MainScreen()
        {
            InitializeComponent();
        }

        public MainScreen(TcpClient client, (string, int) ipAddressTuple)
        {
            _client = client;
            _ipAddressTuple = ipAddressTuple;
        }
    }
}
