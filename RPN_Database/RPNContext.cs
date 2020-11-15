using System.Data.Entity;
using RPN_Database.Model;

namespace RPN_Database
{
    /// <summary>
    /// Kontekst bazy danych kalkulatora RPN.
    /// </summary>
    public class RpnContext : DbContext
    {
        public RpnContext() : base("name=RPNContext") { }
        public DbSet<History> History { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Report> Reports { get; set; }

    }
}
