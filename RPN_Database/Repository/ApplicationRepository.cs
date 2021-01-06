using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics.Contracts;
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

        public async Task<AdminApplication> CreateNewApplication(User user)
        {
            var entity = new AdminApplication
            {
                User = user,
            };

            Applications.Add(entity);

            await SaveChangesAsync();
            return entity;
        }

        public IEnumerable<string> Unresolved()
        {
            var applications = Applications
                .Where(a => a.IsRejected == null)
                .Include(a => a.User)
                .ToList();

            return applications.Select(a => $"{a.Id},{a.User.Username},{a.Created}");
        }

        public IEnumerable<AdminApplication> UnresolvedAsObject()
        {
            var applications = Applications
                .Where(a => a.IsRejected == null)
                .Include(a => a.User)
                .ToList();

            return applications;
        }

        public async void UpdateRejection(int id, bool? value)
        {
            try
            {
                var application = await Applications.FindAsync(id);
                if (application != null) application.IsRejected = value;

                await SaveChangesAsync();
            }
            catch (Exception)
            {

            }
        }
    }
}