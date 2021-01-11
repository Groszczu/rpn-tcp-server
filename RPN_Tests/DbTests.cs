using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RPN_Database;
using RPN_Database.Repository;

namespace RPN_Tests
{
    [TestClass]
    public class DbTests
    {
        private readonly UserRepository _userRepository;

        public DbTests()
        {
            var rpnContext = new ContextBuilder().CreateRpnContext();
            _userRepository = new UserRepository(rpnContext);
        }

        [TestMethod]
        public void CorrectAdminVerification()
        {
            var admin = _userRepository.Users.First(u => u.Username == "admin");
            var user = _userRepository.Users.First(u => u.Username == "user");

            Assert.IsTrue(_userRepository.IsAdmin(admin));
            Assert.IsFalse(_userRepository.IsAdmin(user));
        }
    }
}