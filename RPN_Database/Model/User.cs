using System;
using System.Collections.Generic;
using System.Text;

namespace RPN_Database.Model
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;

        public override string ToString() => $"User{{Id: {Id}, Username: {Username}, Password: {Password}, Created: {Created}}}";
    }
}
