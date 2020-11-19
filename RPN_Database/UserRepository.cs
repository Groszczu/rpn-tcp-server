using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RPN_Database;
using RPN_Database.Model;

using static BCrypt.Net.BCrypt;

namespace RPN_Database
{
    class UserRepository : ContextBasedRepository
    {
        private DbSet<User> AllUsers { get => _context.Users; }

        public HashSet<User> ConnectedUsers { get; }

        public UserRepository(RpnContext rpnContext) : base(rpnContext)
        {
        }

        public User Login(string username, string password)
        {
            var user = AllUsers.FirstOrDefault(u => u.Username == username);

            if (user == null)
            {
                throw new InvalidOperationException("User with given username doesn't exist");
            }

            if (!EnhancedVerify(password, user.Password))
            {
                throw new InvalidOperationException("Invalid password provided");
            }

            if (ConnectedUsers.Contains(user))
            {
                throw new InvalidOperationException("User already logged in");
            }

            ConnectedUsers.Add(user);

            return user;
        }

        public async Task<User> Register(string username, string password)
        {
            var user = AllUsers.FirstOrDefault(u => u.Username == username);

            if(user != null)
            {
                throw new InvalidOperationException("Username taken");
            }

            AllUsers.Add(new User()
            {
                Created = DateTime.Now,
                Username = username,
                Password = EnhancedHashPassword(password)
            });

            await SaveChanges();

            return Login(username, password);
        }

        public void Logout (User user)
        {
            ConnectedUsers.Remove(user);
        }
    }
}
