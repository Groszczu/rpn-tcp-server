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
using RPN_Database.Repository;
using RPN_Locale;

namespace RPN_TcpServer
{
    public class ResponseServerAsync : ResponseServer<double>
    {
        public UserRepository UserRepository { get; }
        public HistoryRepository HistoryRepository { get; }
        public ReportRepository ReportRepository { get; }

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
            ContextCreator<RpnContext> createContext, Action<string> logger = null) : base(localAddress, port, transformer, responseEncoding, logger)
        {
            var context = createContext();
            UserRepository = new UserRepository(context);
            HistoryRepository = new HistoryRepository(context);
            ReportRepository = new ReportRepository(context);
        }

        public override async Task Start()
        {
            await base.Start();

            while (true)
            {
                var tcpClient = await _server.AcceptTcpClientAsync();
                _logger("Client connected");

                var task = ServeClient(tcpClient).ContinueWith(result => { _logger("Client disconnected"); });

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

            await Send(stream, new[] {"You are connected", "Please authenticate"});
            var authInput = await streamReader.ReadLineAsync();

            string authOperationType, username, password, newPassword = string.Empty;

            var authParams = authInput.Split();

            switch (authParams.Length)
            {
                case 3:
                    authOperationType = authParams[0];
                    username = authParams[1];
                    password = authParams[2];
                    break;
                case 4:
                    authOperationType = authParams[0];
                    username = authParams[1];
                    password = authParams[2];
                    newPassword = authParams[3];
                    break;
                default:
                    await Send(stream, "Invalid authentication message format");

                    CloseStreams(streamReader);
                    return;
            }

            User currentUser;

            switch (authOperationType)
            {
                case CoreLocale.Register:
                    try
                    {
                        currentUser = await UserRepository.Register(username, password);
                        break;
                    }
                    catch (InvalidOperationException e)
                    {
                        await Send(stream, e.Message);
                        CloseStreams(streamReader);
                        return;
                    }
                case CoreLocale.Login:
                    try
                    {
                        currentUser = UserRepository.Login(username, password);
                        break;
                    }
                    catch (InvalidOperationException e)
                    {
                        await Send(stream, e.Message);
                        CloseStreams(streamReader);
                        return;
                    }
                case CoreLocale.ChangePassword:
                    try
                    {
                        currentUser = await UserRepository.ChangePassword(username, password, newPassword);
                        await Send(stream, $"Password for user {username} has been changed");
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
                    await Send(stream,
                        new[]
                        {
                            "Enter RPN expression", "'history' to check last inputs", "'exit' to disconnect",
                            "'report <message>' to report a problem"
                        });
                    input = await streamReader.ReadLineAsync();
                }
                catch (Exception)
                {
                    UserRepository.Logout(currentUser);
                    CloseStreams(streamReader);
                    break;
                }

                if (input == CoreLocale.History)
                {
                    if (currentUser.Username == "admin")
                    {
                        await Send(stream, HistoryRepository.All);
                    }
                    else
                    {
                        await Send(stream, HistoryRepository.ById(currentUser.Id));
                    }
                }
                else if (Regex.IsMatch(input, RegularExpression.Report))
                {
                    var match = Regex.Match(input, RegularExpression.ReportWithGroup);
                    var message = match.Groups[RegularExpression.ReportGroup].Value;

                    await ReportRepository.Add(currentUser, message);
                }
                else if (input == CoreLocale.GetReports)
                {
                    if (currentUser.Username == "admin")
                    {
                        await Send(stream, ReportRepository.All);
                    }
                    else
                    {
                        await Send(stream, "Not authorized");
                    }
                }
                else if (input == CoreLocale.Exit)
                {
                    break;
                }
                else
                {
                    try
                    {
                        var result = _transformer(input).ToString();

                        await HistoryRepository.Add(currentUser, input, result);

                        await Send(stream, result);
                    }
                    catch (Exception e)
                    {
                        await Send(stream, $"Calculation error: {e.Message}");
                    }
                }
            }

            UserRepository.Logout(currentUser);
            CloseStreams(streamReader);
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