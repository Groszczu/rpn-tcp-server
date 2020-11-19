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
        [TestMethod]
        public void IPAdressCheckTest()
        {
            var port = 1024;
            var ipAddress = IPAddress.Parse("0.0");
            Assert.ThrowsException<FileNotFoundException>(() => new ResponseServerAsync(ipAddress, port, RPNCalculator.Calculate, Encoding.ASCII, ContextBuilder.CreateRpnContext));
        }

        [TestMethod]
        public void PortNumberCheckTest()
        {
            var port = 443;
            var ipAddress = IPAddress.Parse("127.0.0.1");
            Assert.ThrowsException<FileNotFoundException>(() => new ResponseServerAsync(ipAddress, port, RPNCalculator.Calculate, Encoding.ASCII, ContextBuilder.CreateRpnContext));

        }

        [TestMethod]
        public void NullArgumentTest()
        {
            var port = -2;
            var ipAddress = IPAddress.Parse("127.0.0.1");
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new ResponseServerAsync(ipAddress, port, RPNCalculator.Calculate, Encoding.ASCII, ContextBuilder.CreateRpnContext));

        }
    }
}
