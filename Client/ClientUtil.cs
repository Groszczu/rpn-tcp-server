using System;
using System.Data;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Client.exceptions;
using Exception = System.Exception;

namespace Client
{
    class ClientUtil
    {
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
        /// Obsługuje naciśnięcie przycisku logowania.
        /// </summary>
        /// <exception cref="ServerDownException">Gdy serwer przestał odpowiadać.</exception>
        /// <exception cref="DuplicateNameException">Gdy użytkownik jest już zalogowany.</exception>
        /// <exception cref="ServerDownException">Gdy dane użytkownika nie istnieją w bazie danych serwera.</exception>
        /// <exception cref="ArgumentException">Gdy dane użytkownika są puste.</exception>
        public static async Task HandleLoginProcedure(NetworkStream stream, string username, string password)
        {
            var streamReader = new StreamReader(stream);

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                throw new ArgumentNullException("fields cannot be blank");

            _ = await ReadFromStreamAsync(streamReader); //You are connected
            _ = await ReadFromStreamAsync(streamReader); //Please enter username

            await SendToStreamAsync(stream, username);

            _ = await ReadFromStreamAsync(streamReader); //Please enter password

            await SendToStreamAsync(stream, password);

            var message = await ReadFromStreamAsync(streamReader);

            if (message == "User is already connected")
                throw new DuplicateNameException("a user with given username is already logged in");

            if (message == "User doesn't exist")
                throw new InvalidCredentialException("a user with given credentials does not exist");

            if (message == "Invalid password")
                throw new InvalidCredentialException("wrong password");
        }
    }
}