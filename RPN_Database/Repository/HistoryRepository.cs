using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using RPN_Database.Model;

namespace RPN_Database.Repository
{
    public class HistoryRepository : ContextBasedRepository
    {
        private DbSet<History> History => _context.History;

        public IEnumerable<History> All => History.ToList();

        public HistoryRepository(RpnContext rpnContext) : base(rpnContext)
        {
        }

        public IEnumerable<History> ById(int userId)
        {
            return History.Where(h => h.UserId == userId).ToList();
        }

        public async Task<History> Add(User user, string expresion, string result)
        {
            var newHistory = new History
            {
                Expression = expresion,
                Result = result,
                User = user,
            };

            History.Add(newHistory);

            await SaveChangesAsync();

            return newHistory;
        }
    }
}
