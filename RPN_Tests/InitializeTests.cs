using System;
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
            try
            {
                var port = 1024;
                var ipAddress = IPAddress.Parse("0.0");
                ResponseServer<double> rpnServer = new ResponseServerAsync(ipAddress, port, RPNCalculator.Calculate, Encoding.ASCII, ContextBuilder.CreateRpnContext);
                Assert.Fail();
            }
            catch (ArgumentException e)
            {
                Assert.Fail();
            }
            catch (Exception e)
            {

            }
        }

        [TestMethod]
        public void PortNumberCheckTest()
        {
            try
            {
                var port = 443;
                var ipAddress = IPAddress.Parse("127.0.0.1");
                ResponseServer<double> rpnServer = new ResponseServerAsync(ipAddress, port, RPNCalculator.Calculate, Encoding.ASCII, ContextBuilder.CreateRpnContext);
                Assert.Fail();
            }
            catch (ArgumentException e)
            {
                Assert.Fail();
            }
            catch (Exception e)
            {

            }
        }
    }
}
