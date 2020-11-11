using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using RPN_Calculator;
using RPN_Database;
using RPN_TcpServer;

namespace RPN_TcpServer
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            var (ipAddress, port) = args.Length == 0 
                ? (IPAddress.Loopback, 1024)
                : TryParseConnectionArgs(args);

            if (ipAddress == null || port == 0)
            {
                Console.WriteLine("Please enter following arguments: 1. IP address, 2. port");
                return;
            }

            Console.WriteLine(ipAddress.ToString());
            ResponseServer<double> rpnServer = new ResponseServerAsync(ipAddress, port, RPNCalculator.Calculate, Encoding.ASCII, ContextBuilder.CreateRpnContext);

            await rpnServer.Start();
        }

        private static (IPAddress, int) TryParseConnectionArgs(string[] args)
        {
            try
            {
                var ipAddress = IPAddress.Parse(args[0]);
                var port = int.Parse(args[1]);
                return (ipAddress, port);
            }
            catch (Exception)
            {
                return (null, 0);
            }
        }
    }
}
