﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Client.Exceptions;
using RPN_Locale;
using static Client.Utility.Core;

namespace Client.Utility
{
    public static class Procedures
    {
        /// <summary>
        /// Obsługuje naciśnięcie przycisku logowania lub rejestracji.
        /// </summary>
        /// <param name="stream">Strumień</param>
        /// <param name="authProcedure">Rodzaj operacji</param>
        /// <param name="username">Nazwa użytkownika</param>
        /// <param name="password">Wprowadzone hasło</param>
        /// <exception cref="ServerDownException">Gdy serwer przestał odpowiadać.</exception>
        /// <exception cref="DuplicateNameException">Gdy użytkownik jest już zalogowany.</exception>
        /// <exception cref="ServerDownException">Gdy dane użytkownika nie istnieją w bazie danych serwera.</exception>
        /// <exception cref="ArgumentException">Gdy dane użytkownika są puste.</exception>
        public static async Task HandleAuthenticationProcedure(NetworkStream stream,
            AuthProcedure authProcedure,
            string username,
            string password,
            string newPassword = null)
        {
            var streamReader = new StreamReader(stream);

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                throw new ArgumentNullException("fields cannot be blank");

            _ = await streamReader.ReadLineAsync(); //You are connected
            _ = await streamReader.ReadLineAsync(); //Please authenticate

            string procedure = string.Empty, request = null;

            switch (authProcedure)
            {
                case AuthProcedure.Login:
                    procedure = CoreLocale.Login;
                    break;
                case AuthProcedure.Register:
                    procedure = CoreLocale.Register;
                    break;
                case AuthProcedure.ChangePassword:
                    procedure = CoreLocale.ChangePassword;
                    request = $"{procedure} {username} {password} {newPassword}";
                    break;
            }

            await SendToStreamAsync(stream, request ?? $"{procedure} {username} {password}");

            var message = await streamReader.ReadLineAsync(); //Enter RPN Expression / Error

            switch (message)
            {
                case CoreLocale.UserLoggedIn:
                    throw new DuplicateNameException(message);
                case CoreLocale.UsernameTaken:
                    throw new DuplicateNameException(message);
                case CoreLocale.NoSuchUsername:
                    throw new InvalidCredentialException(message);
                case CoreLocale.InvalidPassword:
                    throw new InvalidCredentialException(message);
                case CoreLocale.NoUser:
                    throw new InvalidCredentialException(message);
            }

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

            if (!double.TryParse(result, NumberStyles.Any, CultureInfo.InvariantCulture, out _))
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
                    expression = CoreLocale.History;
                    regex = RegularExpression.HistoryString;
                    break;
                }
                case Request.Reports:
                {
                    expression = CoreLocale.GetReports;
                    regex = RegularExpression.ReportString;
                    break;
                }
                case Request.Applications:
                    expression = CoreLocale.GetApplications;
                    regex = RegularExpression.ApplicationString;
                    break;
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
            await SendToStreamAsync(stream, CoreLocale.Exit);
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

            await SendToStreamAsync(stream, $"{CoreLocale.Report} {report}");

            _ = await streamReader.ReadLineAsync(); //Enter RPN Expression
            _ = await streamReader.ReadLineAsync(); //'history' to check last inputs
            _ = await streamReader.ReadLineAsync(); //'exit' to disconnect
            _ = await streamReader.ReadLineAsync(); //'report <message>' to report a problem
        }

        /// <summary>
        /// Przeprowadza proces zgłoszenia aplikacji o uprawnienia administratora.
        /// </summary>
        /// <param name="stream">Strumień.</param>
        /// <param name="checkOnly">Zażądaj wyłącznie sprawdzenia uprawnień.</param>
        /// <returns></returns>
        public static string ProcessAdminRequest(NetworkStream stream, bool checkOnly = false)
        {
            var streamReader = new StreamReader(stream);

            SendToStream(stream, checkOnly ? CoreLocale.CheckAdmin : CoreLocale.RequestAdmin);

            var message = streamReader.ReadLine();

            _ = streamReader.ReadLine(); //Enter RPN Expression
            _ = streamReader.ReadLine(); //'history' to check last inputs
            _ = streamReader.ReadLine(); //'exit' to disconnect
            _ = streamReader.ReadLine(); //'report <message>' to report a problem

            return message;
        }
    }
}