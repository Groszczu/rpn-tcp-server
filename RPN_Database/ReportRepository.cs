using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using RPN_Database.Model;

namespace RPN_Database
{
    public class ReportRepository : ContextBasedRepository
    {
        private DbSet<Report> Reports { get => _context.Reports; }

        public IEnumerable<Report> All => Reports.ToList();

        public ReportRepository(RpnContext rpnContext) : base(rpnContext)
        {
        }

        public async Task<Report> Add(User user, string message)
        {
            var newReport = new Report()
            {
                User = user,
                Message = message
            };

            Reports.Add(newReport);

            await SaveChanges();

            return newReport;
        }
    }
}
