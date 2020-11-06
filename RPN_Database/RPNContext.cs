using System;
using System.Data.Entity;
using RPN_Database.Model;

namespace RPN_Database
{
    public class RPNContext : DbContext
    {
        public RPNContext(string connectionString) : base(connectionString)
        {
        }

        public DbSet<History> History { get; set; }
    }
}
