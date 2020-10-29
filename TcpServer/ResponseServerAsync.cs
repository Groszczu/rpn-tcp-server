using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using JSONDataBase;

namespace TcpServer
{
    public class ResponseServerAsync : ResponseServer<double>
    {
        private HashSet<string> _connectedUsers = new HashSet<string>();

        public ResponseServerAsync(IPAddress localAddress, int port, IResponseTransformer<double> transformer, Encoding responseEncoding)
            : base(localAddress, port, transformer, responseEncoding)
        {

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

            var db = JsonDataBase.Create(username);

            Send(stream, "Connected to database\n\r");

            while (true)
            {
                Send(stream, "Enter RPN expression ('history' to check last inputs, 'exit' to disconnect)\n\r");
                var input = streamReader.ReadLine();

                if (input == "history")
                {
                    foreach (var record in db.GetHistory())
                    {
                        Send(stream, record + "\n\r");
                    }
                    continue;
                }
                else if (input == "exit")
                {
                    break;
                }

                try
                {
                    var result = _transformer(input).ToString();

                    db.AddRecord($"{input}: {result}");

                    Send(stream, result + "\n\r");

                }
                catch (Exception e)
                {
                    Send(stream, e.Message);
                }
            }
            db.SaveChanges();
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

        private string ReadAndClearBuffer(NetworkStream stream)
        {
            var chars = stream.Read(_buffer, 0, _buffer.Length);
            var message = _encoding.GetString(_buffer);
            message = message.Substring(0, chars);
            stream.Flush();

            return message;
        }
    }
}
