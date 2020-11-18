using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.exceptions
{
    public class ServerDownException : Exception
    {
        public ServerDownException(string message) : base(message) { }
    }
}
