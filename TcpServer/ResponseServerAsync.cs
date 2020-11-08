using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using RPN_Database;
using RPN_Database.Model;

namespace TcpServer
{
    public class ResponseServerAsync : ResponseServer<double>
    {
        private readonly HashSet<string> _connectedUsers = new HashSet<string>();
        private readonly RPNContext _context;
        protected readonly CreateContext _contextCreator;

        public ResponseServerAsync(IPAddress localAddress, int port, ResponseTransformer<double> transformer, Encoding responseEncoding, CreateContext creator, RPNContext context)
            : base(localAddress, port, transformer, responseEncoding)
        {
            _contextCreator = creator;
            _context = context;
        }

        public override async Task Start()
        {
            await base.Start();
            _contextCreator();
            while (true)
            {
                var tcpClient = await _server.AcceptTcpClientAsync();
                _logger("Client connected");
                var task = ServeClient(tcpClient).ContinueWith((result) => _logger("Client disconnected"));
            }
        }

        private async Task ServeClient(TcpClient client)
        {
            var stream = client.GetStream();
            await Send(stream, "You are connected\n\rPlease enter user name\n\r");
            var streamReader = new StreamReader(stream);

            var username = streamReader.ReadLine();
            if (_connectedUsers.Contains(username))
            {
                await Send(stream, "User already connected");
                streamReader.Close();
                stream.Close();
                return;
            }
            _connectedUsers.Add(username);

            while (true)
            {
                string input;
                try
                {
                    await Send(stream, "Enter RPN expression ('history' to check last inputs, 'exit' to disconnect)\n\r");
                    input = streamReader.ReadLine();
                }
                catch (Exception)
                {
                    streamReader.Close();
                    stream.Close();
                    return;
                }

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

                        await Send(stream, result + "\n\r");
                    }
                    catch (Exception e)
                    {
                        await Send(stream, e.Message);
                    }
                }
            }

            streamReader.Close();
            stream.Close();

            _connectedUsers.Remove(username);
        }

        private Task Send(NetworkStream stream, string message)
        {
            return stream.WriteAsync(_encoding.GetBytes(message), 0, message.Length);
        }
    }
}
