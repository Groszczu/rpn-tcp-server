using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

namespace RPN_Database
{
    public class RPNData
    {
        #region Fields
        SQLiteConnection connect;


        #endregion

        #region Methods
        public void setConn(SQLiteConnection con)
        {
            connect = con;
            connect.Open();
        }

        public void ContextBuilder()
        {
            string workingDirectory = Environment.CurrentDirectory;
            if (File.Exists(workingDirectory + "\\rpn.sqlite"))
            {
                Console.WriteLine("Database exist!");
            }
            else
            {
                Console.WriteLine("Database does not exist! Creating database...");
                setConn(new SQLiteConnection("Data Source=" + workingDirectory + "\\rpn.sqlite"));
                CreateDatabase();
            }
        }

        public void CreateDatabase()
        {
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
        #endregion
    }
}
