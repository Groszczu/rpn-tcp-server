using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Client.exceptions;
using Exception = System.Exception;

namespace Client
{
    public static class ClientUtil
    {
        public static int MinPort { get; } = 1024;
        public static int MaxPort { get; } = 65535;
        public static string RpnRegex { get; } = @"^(\d*\.?\d*)( (\d*\.?\d*) ([\+\-\*\/^%]|root|log))+$";
        public static string HistoryRegex { get; } = @"^History{.*}$";

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

                if (portNumber < MinPort || portNumber > MaxPort)
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
        private static Task SendToStreamAsync(NetworkStream stream, string message)
        {
            var messageLine = $"{message}\r\n";

            return stream.WriteAsync(Encoding.UTF8.GetBytes(messageLine), 0, messageLine.Length);
        }

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

            _ = await streamReader.ReadLineAsync(); //You are connected
            _ = await streamReader.ReadLineAsync(); //Please enter username

            await SendToStreamAsync(stream, username);

            _ = await streamReader.ReadLineAsync(); //Please enter password

            await SendToStreamAsync(stream, password);

            var message = await streamReader.ReadLineAsync(); //Enter RPN Expression / Error

            if (message == "User is already connected")
                throw new DuplicateNameException(message);

            if (message == "User doesn't exist")
                throw new InvalidCredentialException(message);

            if (message == "Invalid password")
                throw new InvalidCredentialException(message);

            _ = await streamReader.ReadLineAsync(); //'history' to check last inputs
            _ = await streamReader.ReadLineAsync(); //'exit' to disconnect
            _ = await streamReader.ReadLineAsync(); //'report <message>' to report a problem
        }

        /// <summary>
        /// Sprawdza czy podane wyrażenie RPN jest poprawne składniowo.
        /// </summary>
        /// <param name="expression">Wyrażenie do sprawdzenia.</param>
        /// <returns></returns>
        public static bool IsValidRpn(string expression) => Regex.IsMatch(expression, RpnRegex);

        /// <summary>
        /// Funkcja rozszerzająca umożliwiająca kolorowanie składni wyrażenia RPN wewnątrz RichTextBox'a.
        /// </summary>
        public static void ValidateAndColorRpnExpression(this RichTextBox textBox)
        {
            var currentText = textBox.Text;
            textBox.Text = String.Empty;

            textBox.SelectionColor = IsValidRpn(currentText) ? Color.Green : Color.Red;
            textBox.AppendText(currentText);
        }

        public static async Task<string> ProcessCalculationRequest(NetworkStream stream, string expression)
        {
            var streamReader = new StreamReader(stream);

            if (!IsValidRpn(expression))
                throw new ArgumentException("invalid rpn expression");

            await SendToStreamAsync(stream, expression);

            var result = await streamReader.ReadLineAsync(); // OK / Error

            _ = await streamReader.ReadLineAsync(); //Enter RPN Expression
            _ = await streamReader.ReadLineAsync(); //'history' to check last inputs
            _ = await streamReader.ReadLineAsync(); //'exit' to disconnect
            _ = await streamReader.ReadLineAsync(); //'report <message>' to report a problem

            if (!double.TryParse(result, out _))
                throw new DataException("returned result is non a number");

            return result;
        }

        public static async Task<List<string>> ProcessHistoryRequest(NetworkStream stream)
        {
            var streamReader = new StreamReader(stream);

            await SendToStreamAsync(stream, "history");

            string line;
            var result = new List<string>();

            while (Regex.IsMatch(line = await streamReader.ReadLineAsync(), HistoryRegex))
            {
                result.Add(line);
            };

            _ = await streamReader.ReadLineAsync(); //'history' to check last inputs
            _ = await streamReader.ReadLineAsync(); //'exit' to disconnect
            _ = await streamReader.ReadLineAsync(); //'report <message>' to report a problem

            return result;
        }


        public static async Task ProcessDisconnectRequest(NetworkStream stream)
        {
            var streamReader = new StreamReader(stream);

            await SendToStreamAsync(stream, "exit");
        }
    }
}