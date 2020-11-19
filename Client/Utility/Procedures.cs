using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Client.Exceptions;

using static Client.Utility.Core;

namespace Client.Utility
{
    public static class Procedures
    {
        public static string HistoryRegex { get; } = @"^History{.*}$";
        public static string ReportRegex { get; } = @"^Report{.*}$";

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
        /// Przeprowadza proces obliczania wyrażenia.
        /// </summary>
        /// <param name="stream">Strumień.</param>
        /// <param name="expression">Wyrażenie w RPN.</param>
        /// <returns>Wynik jako napis.</returns>
        /// <exception cref="ArgumentException">Gdy wyrażenie nie jest poprawnym RPNem.</exception>
        /// <exception cref="DataException">Gdy zwrócony wynik nie jest liczbą.</exception>
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

        /// <summary>
        /// Przeprowadza proces pobierania listy wyników z serwera.
        /// </summary>
        /// <param name="stream">Strumień.</param>
        /// <param name="request">Rodzaj żądania.</param>
        /// <returns>Lista otrzymanych wyników w postaci napisu.</returns>
        /// <exception cref="DataException">Gdy nie otrzymano żadnych wyników.</exception>
        public static async Task<List<string>> ProcessInformationRequest(NetworkStream stream, Request request)
        {
            var streamReader = new StreamReader(stream);
            var expression = string.Empty;
            var regex = string.Empty;

            switch (request)
            {
                case Request.History:
                    {
                        expression = "history";
                        regex = HistoryRegex;
                        break;
                    }
                case Request.Reports:
                    {
                        expression = "get reports";
                        regex = ReportRegex;
                        break;
                    }
            }

            await SendToStreamAsync(stream, expression);

            string line;
            var result = new List<string>();

            while (Regex.IsMatch(line = await streamReader.ReadLineAsync(), regex))
            {
                result.Add(line);
            }

            _ = await streamReader.ReadLineAsync(); //'history' to check last inputs
            _ = await streamReader.ReadLineAsync(); //'exit' to disconnect
            _ = await streamReader.ReadLineAsync(); //'report <message>' to report a problem

            if (result.Count == 0) throw new DataException("no data");

            return result;
        }

        /// <summary>
        /// Przeprowadza proces rozłączania klienta.
        /// </summary>
        /// <param name="stream">Strumień.</param>
        public static async Task ProcessDisconnectRequest(NetworkStream stream)
        {
            await SendToStreamAsync(stream, "exit");
        }

        /// <summary>
        /// Przeprowadza proces zgłaszania błędu.
        /// </summary>
        /// <param name="stream">Strumień.</param>
        /// <param name="report">Zgłoszenie w postaci napisu.</param>
        public static async Task ProcessBugReportRequest(NetworkStream stream, string report)
        {
            var streamReader = new StreamReader(stream);

            if (string.IsNullOrWhiteSpace(report)) throw new ArgumentException("report is null or blank");

            await SendToStreamAsync(stream, $"report {report}");

            _ = await streamReader.ReadLineAsync(); //Enter RPN Expression
            _ = await streamReader.ReadLineAsync(); //'history' to check last inputs
            _ = await streamReader.ReadLineAsync(); //'exit' to disconnect
            _ = await streamReader.ReadLineAsync(); //'report <message>' to report a problem
        }
    }
}
