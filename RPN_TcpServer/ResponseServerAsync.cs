using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using RPN_Database;
using RPN_Database.Model;

using static BCrypt.Net.BCrypt;

namespace RPN_TcpServer
{
    public class ResponseServerAsync : ResponseServer<double>
    {
        private readonly HashSet<User> _connectedUsers;
        private readonly RpnContext _context;

        public ResponseServerAsync(IPAddress localAddress,
                                   int port,
                                   ResponseTransformer<double> transformer,
                                   Encoding responseEncoding,
                                   ContextCreator<RpnContext> createContext) : base(localAddress, port, transformer, responseEncoding)
        {
            _connectedUsers = new HashSet<User>();
            _context = createContext();
        }

        public override async Task Start()
        {
            await base.Start();

            while (true)
            {
                var tcpClient = await _server.AcceptTcpClientAsync();
                _logger("Client connected");

                var task = ServeClient(tcpClient).ContinueWith(result => _logger("Client disconnected"));
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

            await Send(stream, new[] { "You are connected", "Please enter user name" });
            var username = streamReader.ReadLine();

            await Send(stream, "Please enter password");
            var password = streamReader.ReadLine();

            User user;

            try
            {
                user = _context.Users.First(u => u.Username == username);

                if (_connectedUsers.Contains(user))
                {
                    await Send(stream, "User is already connected");
                    CloseStreams(streamReader);
                    return;
                }
                if (!EnhancedVerify(password, user.Password))
                {
                    await Send(stream, "Invalid password");
                    CloseStreams(streamReader);
                    return;
                }
            }
            catch (InvalidOperationException)
            {
                await Send(stream, "User doesn't exist");
                CloseStreams(streamReader);
                return;
            }

            _connectedUsers.Add(user);

            while (true)
            {
                string input;
                try
                {
                    await Send(stream, "Enter RPN expression ('history' to check last inputs, 'exit' to disconnect or 'report <message>' to report a problem)");
                    input = streamReader.ReadLine();
                }
                catch (Exception)
                {
                    CloseStreams(streamReader);
                    return;
                }

                if (input == "history")
                {
                    if (user.Username == "admin")
                    {
                        await _context.History.ForEachAsync(async h => await Send(stream, h.ToString()));
                    }
                    else
                    {
                        await _context.History.Where(h => h.UserId == user.Id).ForEachAsync(async h => await Send(stream, h.ToString()));
                    }
                }
                else if (Regex.IsMatch(input, @"^report\s.*"))
                {
                    var match = Regex.Match(input, @"^report\s(?<message>.*)");
                    var message = match.Groups["message"].Value;

                    _context.Reports.Add(new Report { Message = message, User = user });
                    await _context.SaveChangesAsync();
                }
                else if (input == "get reports")
                {
                    if (user.Username == "admin")
                    {
                        await _context.Reports.ForEachAsync(async r => await Send(stream, r.ToString())); 
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

                        _context.History.Add(new History
                        {
                            Expression = input,
                            Result = result,
                            User = user
                        });

                        await _context.SaveChangesAsync();
                        await Send(stream, result);
                    }
                    catch (Exception e)
                    {
                        await Send(stream, e.Message);
                    }
                }
            }

            CloseStreams(streamReader);
            _connectedUsers.Remove(user);
        }

        private Task Send(NetworkStream stream, string message)
        {
            var messageLine = $"{message}\n\r";
            return stream.WriteAsync(_encoding.GetBytes(messageLine), 0, messageLine.Length);
        }

        private Task Send(NetworkStream stream, string[] messages)
        {
            var messageLine = string.Join("\n\r", messages);
            return Send(stream, messageLine);
        }

        private void CloseStreams(StreamReader reader)
        {
            var stream = reader.BaseStream;

            reader.Close();
            stream.Close();
        }
    }
}
