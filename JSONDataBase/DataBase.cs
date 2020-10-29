using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace JSONDataBase
{
    public class DataBase
    {
        [JsonPropertyName("root")]
        public IDictionary<string, IEnumerable<string>> Root {get; set; }
    }
}
