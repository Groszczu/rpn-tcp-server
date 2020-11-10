using System;
using System.Data.SQLite;
using System.IO;

namespace RPN_Database
{
    public static class ContextBuilder
    {
        #region Methods
        public static RpnContext CreateRpnContext()
        {
            string workingDirectory = Environment.CurrentDirectory;
            if (File.Exists(workingDirectory + "\\rpn.sqlite"))
            {
                Console.WriteLine("Database exist!");
            }
            else
            {
                SQLiteConnection connect = new SQLiteConnection("Data Source=" + workingDirectory + "\\rpn.sqlite");
                connect.Open();
                Console.WriteLine("Database does not exist! Creating database...");

                var cmd = connect.CreateCommand();

                cmd.CommandText =
                                @"CREATE TABLE Users ( Id INTEGER PRIMARY KEY AUTOINCREMENT, 
                            Username VARCHAR(64) NOT NULL UNIQUE, 
                            Password VARCHAR(64) NOT NULL,
                            Created DATE NOT NULL);";
                cmd.ExecuteNonQuery();

                cmd.CommandText =
                                @"CREATE TABLE Histories ( Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            Expression TEXT NOT NULL,
                            Result TEXT NOT NULL,
                            Executed DATE NOT NULL);";
                cmd.ExecuteNonQuery();
            }

            return new RpnContext();
        }
        #endregion
    }
}
