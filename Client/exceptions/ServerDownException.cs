using System;

namespace Client.Exceptions
{
    public class ServerDownException : Exception
    {
        public ServerDownException(string message) : base(message) { }
    }
}
