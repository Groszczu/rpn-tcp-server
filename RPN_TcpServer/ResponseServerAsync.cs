﻿using System;
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

                ServeClient(tcpClient).ContinueWith(result => _logger("Client disconnected"));
            }
        }

        private async Task ServeClient(TcpClient client)
        {
            var stream = client.GetStream();
            var streamReader = new StreamReader(stream);

            await Send(stream, "You are connected\n\rPlease enter user name\n\r");
            var username = streamReader.ReadLine();

            await Send(stream, "Please enter password\n\r");
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
                    await Send(stream, "Enter RPN expression ('history' to check last inputs, 'exit' to disconnect)\n\r");
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
                        await _context.History.ForEachAsync(async h => await Send(stream, $"{h}\n\r"));
                    }
                    else
                    {
                        await _context.History.Where(h => h.UserId == user.Id).ForEachAsync(async h => await Send(stream, $"{h}\n\r"));
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
                        await Send(stream, $"{result}\n\r");
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
            return stream.WriteAsync(_encoding.GetBytes(message), 0, message.Length);
        }

        private void CloseStreams(StreamReader reader)
        {
            var stream = reader.BaseStream;

            reader.Close();
            stream.Close();
        }
    }
}