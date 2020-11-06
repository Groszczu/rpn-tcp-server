using System;
using System.Data.Entity;
using RPN_Database.Model;

namespace RPN_Database
{
    public class RPNContext : DbContext
    {
        public RPNContext() : base("name=RPNContext")
        {
        }

        public DbSet<History> History { get; set; }
    }
}
