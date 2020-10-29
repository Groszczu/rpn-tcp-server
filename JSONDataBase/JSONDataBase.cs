using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using JSONDataBase;

namespace RPN_TcpServerAsync.JSONDataBase
{
    public class JSONDataBase : Core.IHistoryDataBase
    {
        private const string Path = "historyDataBase.json";
        private readonly string _key;
        private readonly DataBase _db;

        private JSONDataBase(string key, DataBase db)
        {
            _key = key;
            _db = db;
        }

        async public static Task<JSONDataBase> Create(string username, string password)
        {
            var key = $"{username}:{password}";
            using (var fs = File.Open(Path, FileMode.OpenOrCreate))
            {
                var db = await JsonSerializer.DeserializeAsync<DataBase>(fs);
                return new JSONDataBase(key, db);
            };
        }

        public void ClearHistory()
        {
            _db.Root[_key] = new List<string>();
        }

        public IEnumerable<string> GetHistory()
        {
            return _db.Root[_key];
        }

        public void SaveRecord(string record)
        {
            if (_db.Root.ContainsKey(_key))
            {
                _db.Root[_key].Prepend(record);
            }
            else
            {
                _db.Root[_key] = new List<string>() { record };
            }
        }
    }
}
