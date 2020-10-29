using System;
using System.Net;
using System.Text;
using TcpServer;

namespace RPN_TcpServer
{
	class Program
	{
		public static void Main(string[] args)
		{
			var (ipAddress, port) = TryParseConnectionArgs(args);

			if (ipAddress == null || port == 0)
			{
				Console.WriteLine("Please enter following arguments: 1. IP address, 2. port");
				return;
			}

			var rpnServer = new ResponseServer<double>(ipAddress, port, RPNCalculator.Calculate, Encoding.ASCII);

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
