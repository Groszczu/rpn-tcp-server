import sqlite3

conn = None

try:
    conn = sqlite3.connect('./RPN_TcpServer/bin/Debug/rpn.sqlite')
except Exception:
    print("Error")
    exit(1)

conn.cursor().execute('DROP TABLE IF EXISTS "Users";')
conn.cursor().execute('DROP TABLE IF EXISTS "Histories";')

conn.cursor().execute('''CREATE TABLE "Users" (
    Id INTEGER PRIMARY KEY AUTOINCREMENT, 
    Username VARCHAR(64) NOT NULL UNIQUE, 
    Password VARCHAR(64) NOT NULL, 
    Created DATE NOT NULL
);
''')

conn.cursor().execute('''CREATE TABLE "Histories" (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Expression TEXT NOT NULL,
    Result TEXT NOT NULL,
    Executed DATE NOT NULL

    --UserId INTEGER NOT NULL REFERENCES Users(id)
);
''')

conn.close()

print("Done")