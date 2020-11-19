using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Contexts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RPN_Database.Model;

namespace RPN_Tests
{
    [TestClass]
    public class CalculationsTests
    {
        [TestMethod]
        public void CalcTests()
        {
            Assert.ThrowsException<IndexOutOfRangeException>(() => RPN_Calculator.RPNCalculator.Calculate("2 2"));

            Assert.ThrowsException<KeyNotFoundException>(() => RPN_Calculator.RPNCalculator.Calculate(""));

            Assert.ThrowsException<DivideByZeroException>(() => RPN_Calculator.RPNCalculator.Calculate("0 8 root"));

            Assert.ThrowsException<DivideByZeroException>(() => RPN_Calculator.RPNCalculator.Calculate("-1 0 root"));

            Assert.ThrowsException<InvalidOperationException>(() => RPN_Calculator.RPNCalculator.Calculate("root"));

            Assert.ThrowsException<KeyNotFoundException>(() => RPN_Calculator.RPNCalculator.Calculate("1.1 0.0 log"));

            Assert.ThrowsException<InvalidOperationException>(() => RPN_Calculator.RPNCalculator.Calculate("log 0 9"));

            Assert.ThrowsException<InvalidOperationException>(() => RPN_Calculator.RPNCalculator.Calculate("log"));

        }
    }
}
