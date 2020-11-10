using System.Data.Entity;
using RPN_Database.Model;

namespace RPN_Database
{
    public class RpnContext : DbContext
    {
        public RpnContext() : base("name=RPNContext") { }

        public DbSet<History> History { get; set; }
        public DbSet<User> Users { get; set; }

    }
}
