using System;
using System.IO;
using System.Net;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RPN_Calculator;
using RPN_Database;
using RPN_TcpServer;

namespace RPN_Tests
{
    [TestClass]
    public class InitializeTests
    {
        readonly ContextBuilder _contextBuilder = new ContextBuilder();

        [TestMethod]
        public void ServerCreatesInstance()
        {
            var port = 1024;
            var ipAddress = IPAddress.Parse("0.0");
            new ResponseServerAsync(ipAddress, port, RPNCalculator.GetResult, Encoding.ASCII,
                _contextBuilder.CreateRpnContext);
        }

        [TestMethod]
        public void NullArgumentTest()
        {
            var port = -2;
            var ipAddress = IPAddress.Parse("127.0.0.1");
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new ResponseServerAsync(ipAddress, port,
                RPNCalculator.GetResult, Encoding.ASCII, _contextBuilder.CreateRpnContext));
        }
    }
}