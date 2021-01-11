using System.Threading.Tasks;

namespace RPN_Database.Repository
{
    public abstract class ContextBasedRepository
    {
        protected readonly RpnContext _context;

        protected ContextBasedRepository(RpnContext rpnContext)
        {
            _context = rpnContext;
        }

        protected Task SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}
