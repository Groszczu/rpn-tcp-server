import sqlite3

conn = None

try:
    conn = sqlite3.connect("./rpn.sqlite")
except Exception:
    exit(1)

conn.cursor().execute("DROP TABLE IF EXISTS History;")
conn.cursor().execute("DROP TABLE IF EXISTS Users;")

conn.cursor().execute("""CREATE TABLE Users (
    id INTEGER PRIMARY KEY AUTOINCREMENT, 
    username VARCHAR(64) NOT NULL UNIQUE, 
    password VARCHAR(64) NOT NULL, 
    created DATE NOT NULL DEFAULT CURRENT_TIMESTAMP
);
""")

conn.cursor().execute("""CREATE TABLE History (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    expression VARCHAR(255) NOT NULL,
    result VARCHAR(255) NOT NULL,
    executed DATE NOT NULL DEFAULT CURRENT_TIMESTAMP,

    user_id INTEGER NOT NULL REFERENCES Users(id)
);
""")

conn.close()