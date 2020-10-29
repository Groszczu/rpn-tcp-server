using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TcpServer
{
	/// <summary>
	/// Class that listens for TCP connections and sends processed response back to client
	/// </summary>
	/// <typeparam name="TResponse">Type of response that callback returns</typeparam>
	public class ResponseServer<TResponse>
	{
		private readonly IResponseTransformer<TResponse> _transformer;
		private readonly TcpListener _server;
		private readonly Encoding _encoding;
		private readonly Action<string> _logger;

		/// <summary>
		/// Basic constructor of ResponseServer class
		/// Creates new ResponseServer instance
		/// </summary>
		/// <param name="localAddress">IP address that server is listening on</param>
		/// <param name="port">Port that server is listening on</param>
		/// <param name="transformer">Delegate that transforms received message</param>
		/// <param name="responseEncoding">Encoding that server will use during reading and writing</param>
		public ResponseServer(IPAddress localAddress, int port, IResponseTransformer<TResponse> transformer, Encoding responseEncoding, Action<string> logger = null)
		{
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
		/// <param name="bufferSize">Size of the message buffer. Defaults to 1024</param>
		public void Start(int bufferSize = 1024)
		{
			_server.Start();
			_logger($"Listening on: {_server.LocalEndpoint}");
			while (true)
			{
				var client = _server.AcceptTcpClient();
				var stream = client.GetStream();

				var buffer = new byte[bufferSize];
				stream.Read(buffer, 0, bufferSize);

				var deserializedRequest = _encoding.GetString(buffer, 0, buffer.Length);
				_logger($"Request: {deserializedRequest}");

				string response;
				try
				{
					var result = _transformer(deserializedRequest);
					response = result.ToString();
				}
				catch (Exception e)
				{
					response = $"Error: {e.Message}";
				}

				_logger($"Response: {response}");
				var serializedResponse = _encoding.GetBytes(response);
				stream.Write(serializedResponse, 0, serializedResponse.Length);
			}
		}
	}
}
