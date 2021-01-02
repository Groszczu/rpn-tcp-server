using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using RPN_Database.Model;

namespace RPN_Database.Repository
{
    public class ApplicationRepository : ContextBasedRepository
    {
        public DbSet<AdminApplication> Applications => _context.Applications;

        public IEnumerable<AdminApplication> All => Applications.ToList();

        public ApplicationRepository(RpnContext rpnContext) : base(rpnContext)
        {
        }

        public async Task<AdminApplication> Add(User user)
        {
            var entity = new AdminApplication
            {
                User = user,
            };

            Applications.Add(entity);

            await SaveChangesAsync();
            return entity;
        }

        public IEnumerable<AdminApplication> Unresolved()
        {
            return Applications.Where(a => a.IsRejected == null).ToList();
        }
    }
}