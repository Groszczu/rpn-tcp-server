using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Exception = System.Exception;

namespace Client
{
    class ClientUtil
    {
        private static readonly int TASK_TIMEOUT = 500;
        private static readonly int MIN_PORT = 1024;
        private static readonly int MAX_PORT = 65535;

        /// <summary>
        /// Metoda sprawdzająca poprawność adresu IP i portu i zwracająca go jeśli jest on poprawny.
        /// </summary>
        /// <param name="ip">Adres IP jako napis</param>
        /// <param name="port">Port jako napis</param>
        /// <returns>Krotkę adresu w formie napisu i portu jako liczby.</returns>
        /// <exception cref="ArgumentException">Gdy podczas walidacji wystąpi błąd.</exception>
        public static (string, int) ParseConnectionArgs(string ip, string port)
        {
            try
            {
                _ = IPAddress.Parse(ip);
                var portNumber = int.Parse(port);

                if (portNumber < MIN_PORT || portNumber > MAX_PORT)
                {
                    throw new ArgumentException("invalid port number");
                }

                return (ip, portNumber);
            }
            catch (Exception e)
            {
                throw new ArgumentException($"error while parsing data: {e.Message}");
            }
        }

        /// <summary>
        /// Wysyła wiadomość do strumienia.
        /// </summary>
        /// <param name="message">Wiadomość do wysłania.</param>
        /// <returns>Zadanie wysyłające wiadomość do strumienia.</returns>
        public static Task SendToStreamAsync(NetworkStream stream, string message)
        {
            var messageLine = $"{message}\r\n";

            return stream.WriteAsync(Encoding.UTF8.GetBytes(messageLine), 0, messageLine.Length);
        }

        /// <summary>
        /// Odczytuje linię ze strumienia.
        /// </summary>
        /// <returns>Zadanie odczytujące linię.</returns>
        public static Task<string> ReadFromStreamAsync(StreamReader reader) => reader.ReadLineAsync();

        /// <summary>
        /// Odczytuje wszystkie zaległe wiadomości ze strumienia.
        /// </summary>
        /// <returns>Zadanie zwracające ostatnią odczytaną wiadomość.</returns>
        public static async Task<string> FlushStreamAsync(StreamReader reader)
        {
            using (var cancellationToken = new CancellationTokenSource())
            {
                string lastMessage = "";

                while (true)
                {
                    var task = reader.ReadLineAsync();
                    var completedTask = await Task.WhenAny(task, Task.Delay(TASK_TIMEOUT, cancellationToken.Token));

                    if (completedTask == task)
                    {
                        cancellationToken.Cancel();
                        lastMessage = await task;
                    }

                    return lastMessage;
                }
            }
        }
    }
}
