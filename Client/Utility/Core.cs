using System;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using RPN_Locale;
using Exception = System.Exception;

namespace Client.Utility
{
    public static class Core
    {
        private static int MinPort => 1024;
        private static int MaxPort => 65535;

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
        /// Wysyła wiadomość do strumienia asynchronicznie.
        /// </summary>
        /// <param name="stream">Strumień.</param>
        /// <param name="message">Wiadomość do wysłania.</param>
        /// <param name="encoding" default="UTF8">Wykorzystywane enkodowanie.</param>
        /// 
        /// <returns>Zadanie wysyłające wiadomość do strumienia.</returns>
        public static Task SendToStreamAsync(NetworkStream stream, string message, Encoding encoding = null)
        {
            encoding = encoding ?? Encoding.UTF8;

            var messageLine = $"{message}\r\n";

            return stream.WriteAsync(encoding.GetBytes(messageLine), 0, messageLine.Length);
        }
        
        /// <summary>
        /// Wysyła wiadomość do strumienia.
        /// </summary>
        /// <param name="stream">Strumień.</param>
        /// <param name="message">Wiadomość do wysłania.</param>
        /// <param name="encoding" default="UTF8">Wykorzystywane enkodowanie.</param>
        /// 
        /// <returns>Zadanie wysyłające wiadomość do strumienia.</returns>
        public static void SendToStream(NetworkStream stream, string message, Encoding encoding = null)
        {
            encoding = encoding ?? Encoding.UTF8;

            var messageLine = $"{message}\r\n";

            stream.Write(encoding.GetBytes(messageLine), 0, messageLine.Length);
        }

        /// <summary>
        /// Sprawdza czy podane wyrażenie RPN jest poprawne składniowo.
        /// </summary>
        /// <param name="expression">Wyrażenie do sprawdzenia.</param>
        public static bool IsValidRpn(string expression) => Regex.IsMatch(expression, RegularExpression.Rpn);

        /// <summary>
        /// Funkcja rozszerzająca umożliwiająca kolorowanie składni wyrażenia RPN wewnątrz RichTextBox'a.
        /// </summary>
        public static void HighlightRpnExpression(this RichTextBox textBox)
        {
            var currentText = textBox.Text;
            textBox.Text = String.Empty;

            textBox.SelectionColor = IsValidRpn(currentText) ? Color.Green : Color.Red;
            textBox.AppendText(currentText);
        }
    }
}