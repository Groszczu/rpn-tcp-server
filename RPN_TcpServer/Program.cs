using System;
using System.Net;
using System.Text;
using TcpServer;

namespace TcpServer
{
    class Program
    {
        public static void Main(string[] args)
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
            ResponseServer<double> rpnServer = new ResponseServerAsync(ipAddress, port, RPNCalculator.Calculate, Encoding.ASCII);

            rpnServer.Start();
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
