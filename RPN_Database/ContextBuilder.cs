﻿using System;
using System.Data.SQLite;
using System.IO;
using static BCrypt.Net.BCrypt;

namespace RPN_Database
{
    public class ContextBuilder
    {
        private readonly Action<string> _logger;

        public ContextBuilder(Action<string> logger = null)
        {
            _logger = logger ?? Console.WriteLine;
        }
        
        /// <summary>
        /// Metoda zapewniająca utworzenie bazy danych i kontekstu do pobierania danych.
        /// </summary>
        /// <returns>Kontekst bazy danych kalkulatora RPN.</returns>
        public RpnContext CreateRpnContext()
        {
            string workingDirectory = Environment.CurrentDirectory;
            if (File.Exists($"{workingDirectory}/rpn.sqlite"))
            {
                _logger("Database exist!");
            }
            else
            {
                SQLiteConnection conn = new SQLiteConnection($"Data Source={workingDirectory}/rpn.sqlite");

                conn.Open();
                _logger("Database does not exist! Creating database...");

                var cmd = conn.CreateCommand();

                cmd.CommandText = @"CREATE TABLE Users (
                                    Id INTEGER PRIMARY KEY AUTOINCREMENT, 
                                    Username VARCHAR(64) NOT NULL UNIQUE, 
                                    Password VARCHAR(64) NOT NULL,
                                    Created DATE NOT NULL);";

                cmd.ExecuteNonQuery();

                cmd.CommandText = @"CREATE TABLE Histories (
                                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                    Expression TEXT NOT NULL,
                                    Result TEXT NOT NULL,
                                    Executed DATE NOT NULL,
                                    UserId INTEGER NOT NULL REFERENCES Users(Id));";

                cmd.ExecuteNonQuery();

                cmd.CommandText = @"CREATE TABLE Reports (
                                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                    Message TEXT NOT NULL,
                                    CreatedAt DATE NOT NULL,
                                    UserId INTEGER NOT NULL REFERENCES Users(Id));";

                cmd.ExecuteNonQuery();

                cmd.CommandText = @"CREATE TABLE AdminApplications (
                                    Id INTEGER PRIMARY KEY AUTOINCREMENT, 
                                    UserId INTEGER NOT NULL REFERENCES Users(Id) UNIQUE, 
                                    IsRejected BOOL,
                                    Created DATE NOT NULL);";

                cmd.ExecuteNonQuery();

                cmd.CommandText =
                    $@"INSERT INTO Users (Username, Password, Created) VALUES ('admin', '{EnhancedHashPassword("admin")}', CURRENT_TIMESTAMP);";

                cmd.ExecuteNonQuery();
                
                cmd.CommandText =
                    $@"INSERT INTO AdminApplications (UserId, IsRejected, Created) VALUES (1, FALSE, CURRENT_TIMESTAMP);";

                cmd.ExecuteNonQuery();

                cmd.CommandText =
                    $@"INSERT INTO Users (Username, Password, Created) VALUES ('user', '{EnhancedHashPassword("user")}', CURRENT_TIMESTAMP);";

                cmd.ExecuteNonQuery();

                cmd.CommandText =
                    $@"INSERT INTO Users (Username, Password, Created) VALUES ('user1', '{EnhancedHashPassword("user1")}', CURRENT_TIMESTAMP);";

                cmd.ExecuteNonQuery();

                cmd.CommandText =
                    $@"INSERT INTO Users (Username, Password, Created) VALUES ('user2', '{EnhancedHashPassword("user2")}', CURRENT_TIMESTAMP);";

                cmd.ExecuteNonQuery();
            }

            return new RpnContext();
        }
    }
}