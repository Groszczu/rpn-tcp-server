using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using RPN_Database;
using RPN_Database.Model;

namespace TcpServer
{
    public class ResponseServerAsync : ResponseServer<double>
    {
        private readonly HashSet<string> _connectedUsers = new HashSet<string>();
        private readonly RPNContext _context;

        public ResponseServerAsync(IPAddress localAddress, int port, IResponseTransformer<double> transformer, Encoding responseEncoding, RPNContext context)
            : base(localAddress, port, transformer, responseEncoding)
        {
            _context = context;
        }

        public override void Start()
        {
            base.Start();

            while (true)
            {
                var tcpClient = _server.AcceptTcpClient();
                _logger("Client connected");
                var transmissionDelegate = new Action<TcpClient>(ServeClient);
                transmissionDelegate.BeginInvoke(tcpClient, TransmissionCallback, tcpClient);
            }

        }

        private void ServeClient(TcpClient client)
        {
            var stream = client.GetStream();
            Send(stream, "You are connected\n\rPlease enter user name\n\r");

            var streamReader = new StreamReader(stream);

            var username = streamReader.ReadLine();
            if (_connectedUsers.Contains(username))
            {
                Send(stream, "User already connected");
                streamReader.Close();
                stream.Close();
                return;
            }
            _connectedUsers.Add(username);

            while (true)
            {
                Send(stream, "Enter RPN expression ('history' to check last inputs, 'exit' to disconnect)\n\r");
                var input = streamReader.ReadLine();

                if (input == "history")
                {
                    _context.History.ToList().ForEach(h =>
                    {
                        Send(stream, h + "\n\r");
                    });
                }
                else if (input == "exit")
                {
                    break;
                }
                else
                {
                    try
                    {
                        var result = _transformer(input).ToString();

                        _context.History.Add(new History
                        {
                            Expression = input,
                            Result = result
                        });

                        _context.SaveChanges();

                        Send(stream, result + "\n\r");
                    }
                    catch (ArgumentException e)
                    {
                        Send(stream, e.Message);
                    }
                }
            }

            streamReader.Close();
            stream.Close();

            _connectedUsers.Remove(username);
        }

        private void TransmissionCallback(IAsyncResult _asyncResult)
        {
            _logger("Client disconnected");
        }

        private void Send(NetworkStream stream, string message)
        {
            stream.Write(_encoding.GetBytes(message), 0, message.Length);
        }
    }
}
