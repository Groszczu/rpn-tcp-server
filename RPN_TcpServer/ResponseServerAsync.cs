using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using RPN_Database;
using RPN_Database.Model;

namespace RPN_TcpServer
{
    public class ResponseServerAsync : ResponseServer<double>
    {
        private readonly UserRepository _userRepository;
        private readonly HistoryRepository _historyRepository;
        private readonly ReportRepository _reportRepository;

        /// <summary>
        /// Konstruktor klasy asynchronicznego serwera kalkulacji RPN.
        /// </summary>
        /// <param name="localAddress">Adres IP portu do nasłuchiwania.</param>
        /// <param name="port">Port do nasłuchiwania.</param>
        /// <param name="transformer">Funkcja przeprowadzająca obliczenia RPN.</param>
        /// <param name="responseEncoding">Enkodowanie wykorzystywane do przeprowadzania komunikacji.</param>
        /// <param name="createContext">Funkcja tworząca kontekst bazy danych kalkulatora RPN.</param>
        public ResponseServerAsync(IPAddress localAddress,
                                   int port,
                                   ResponseTransformer<double> transformer,
                                   Encoding responseEncoding,
                                   ContextCreator<RpnContext> createContext) : base(localAddress, port, transformer, responseEncoding)
        {
            var context = createContext();
            _userRepository = new UserRepository(context);
            _historyRepository = new HistoryRepository(context);
            _reportRepository = new ReportRepository(context);
        }

        public override async Task Start()
        {
            await base.Start();

            while (true)
            {
                var tcpClient = await _server.AcceptTcpClientAsync();
                _logger("Client connected");

                var task = ServeClient(tcpClient).ContinueWith(result =>
                {
                    _logger("Client disconnected");
                });

                if (task.IsFaulted)
                {
                    await task;
                }
            }
        }

        private async Task ServeClient(TcpClient client)
        {
            var stream = client.GetStream();
            var streamReader = new StreamReader(stream);

            await Send(stream, new[] { "You are connected", "Please authenticate" });
            var authInput = await streamReader.ReadLineAsync();

            var authParams = authInput.Split();

            if (authParams.Length != 3)
            {
                await Send(stream, "Invalid authentication message format");

                CloseStreams(streamReader);
                return;
            }

            var authOperationType = authParams[0];
            var username = authParams[1];
            var password = authParams[2];

            User currentUser;

            switch (authOperationType)
            {
                case "register":
                    try
                    {
                        currentUser = await _userRepository.Register(username, password);
                        break;
                    }
                    catch (InvalidOperationException e)
                    {
                        await Send(stream, e.Message);
                        CloseStreams(streamReader);
                        return;
                    }
                case "login":
                    try
                    {
                        currentUser = _userRepository.Login(username, password);
                        break;
                    }
                    catch (InvalidOperationException e)
                    {
                        await Send(stream, e.Message);
                        CloseStreams(streamReader);
                        return;
                    }
                default:
                    await Send(stream, "Invalid authentication message format");

                    CloseStreams(streamReader);
                    return;
            }

            while (true)
            {
                string input;
                try
                {
                    await Send(stream, new[] { "Enter RPN expression", "'history' to check last inputs", "'exit' to disconnect", "'report <message>' to report a problem" });
                    input = await streamReader.ReadLineAsync();
                }
                catch (Exception)
                {
                    CloseStreams(streamReader);
                    break;
                }

                if (input == "history")
                {
                    if (currentUser.Username == "admin")
                    {
                        await Send(stream, _historyRepository.All);
                    }
                    else
                    {
                        await Send(stream, _historyRepository.ById(currentUser.Id));
                    }
                }
                else if (Regex.IsMatch(input, @"^report\s.*"))
                {
                    var match = Regex.Match(input, @"^report\s(?<message>.*)");
                    var message = match.Groups["message"].Value;

                    await _reportRepository.Add(currentUser, message);
                }
                else if (input == "get reports")
                {
                    if (currentUser.Username == "admin")
                    {
                        await Send(stream, _reportRepository.All);
                    }
                    else
                    {
                        await Send(stream, "Not authorized");
                    }
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

                        await _historyRepository.Add(currentUser, input, result);

                        await Send(stream, result);
                    }
                    catch (Exception e)
                    {
                        await Send(stream, e.Message);
                    }
                }
            }

            CloseStreams(streamReader);
            _userRepository.Logout(currentUser);
        }

        private Task Send(NetworkStream stream, IEnumerable<object> models)
        {
            return Send(stream, models.Select(m => m.ToString()));
        }
        private Task Send(NetworkStream stream, IEnumerable<string> lines)
        {
            return Send(stream, string.Join("\r\n", lines));
        }

        private Task Send(NetworkStream stream, string message)
        {
            var messageLine = $"{message}\r\n";
            return stream.WriteAsync(_encoding.GetBytes(messageLine), 0, messageLine.Length);
        }

        private void CloseStreams(StreamReader reader)
        {
            var stream = reader.BaseStream;

            reader.Close();
            stream.Close();
        }
    }
}
