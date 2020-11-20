using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RPN_Database
{
    public abstract class ContextBasedRepository
    {
        protected readonly RpnContext _context;

        protected ContextBasedRepository(RpnContext rpnContext)
        {
            _context = rpnContext;
        }

        protected Task SaveChanges()
        {
            return _context.SaveChangesAsync();
        }
    }
}
