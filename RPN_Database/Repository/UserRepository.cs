using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using RPN_Database.Model;
using static BCrypt.Net.BCrypt;
using RPN_Locale;

namespace RPN_Database.Repository
{
    public class UserRepository : ContextBasedRepository
    {
        private DbSet<User> Users => _context.Users;

        public HashSet<User> ConnectedUsers { get; }

        public UserRepository(RpnContext rpnContext) : base(rpnContext)
        {
            ConnectedUsers = new HashSet<User>();
        }

        public User Login(string username, string password)
        {
            var user = Users.FirstOrDefault(u => u.Username == username);

            if (user == null)
            {
                throw new InvalidOperationException(CoreLocale.NoSuchUsername);
            }

            if (!EnhancedVerify(password, user.Password))
            {
                throw new InvalidOperationException(CoreLocale.InvalidPassword);
            }

            if (ConnectedUsers.Contains(user))
            {
                throw new InvalidOperationException(CoreLocale.UserLoggedIn);
            }

            ConnectedUsers.Add(user);

            return user;
        }

        public async Task<User> Register(string username, string password)
        {
            var user = Users.FirstOrDefault(u => u.Username == username);

            if (user != null)
            {
                throw new InvalidOperationException(CoreLocale.UsernameTaken);
            }

            Users.Add(new User()
            {
                Created = DateTime.Now,
                Username = username,
                Password = EnhancedHashPassword(password)
            });

            await SaveChanges();

            return Login(username, password);
        }

        public async Task<User> ChangePassword(string username, string password, string newpassword)
        {
            try
            {
                var user = Login(username, password);

                user.Password = newpassword;
                
                await SaveChanges();

                return user;
            }
            catch
            {
                throw new InvalidOperationException("User does not exist");
            }
        }

        public void Logout(User user)
        {
            ConnectedUsers.Remove(user);
        }
    }
}