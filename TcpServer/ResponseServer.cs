using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TcpServer
{
	/// <summary>
	/// Class that listens for TCP connections and sends processed response back to client
	/// </summary>
	/// <typeparam name="TResponse">Type of response that callback returns</typeparam>
	public abstract class ResponseServer<TResponse>
	{
		protected readonly ResponseTransformer<TResponse> _transformer;
		protected readonly TcpListener _server;
		protected readonly Encoding _encoding;
		protected readonly Action<string> _logger;
		protected readonly IPAddress _iPAddress;
		protected readonly int _port;
		protected readonly byte[] _buffer = new byte[1024];

		/// <summary>
		/// Basic constructor of ResponseServer class
		/// Creates new ResponseServer instance
		/// </summary>
		/// <param name="localAddress">IP address that server is listening on</param>
		/// <param name="port">Port that server is listening on</param>
		/// <param name="transformer">Delegate that transforms received message</param>
		/// <param name="responseEncoding">Encoding that server will use during reading and writing</param>
		public ResponseServer(IPAddress localAddress, int port, ResponseTransformer<TResponse> transformer, Encoding responseEncoding, Action<string> logger = null)
		{
			_iPAddress = localAddress;
			_port = port;
			_logger = logger ?? Console.WriteLine;
			_transformer = transformer;
			_server = new TcpListener(localAddress, port);
			_encoding = responseEncoding;
		}

		/// <summary>
		/// Starts listening on IP address and port specified in constructor
		/// After receiving message uses encoding to parse it to string and pass it as input
		/// to callback function. Result of callback is sent back to client
		/// </summary>
		public virtual async Task Start()
        {
			_logger($"Listening on {_iPAddress}:{_port}");
			_server.Start();
        }
	}
}
