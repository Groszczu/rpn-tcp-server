using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RPN_Tests
{
    [TestClass]
    public class CalculationsTests
    {
        [TestMethod]
        public void CalcTests()
        {
            Assert.ThrowsException<IndexOutOfRangeException>(() => RPN_Calculator.RPNCalculator.GetResult("2 2"));

            Assert.ThrowsException<KeyNotFoundException>(() => RPN_Calculator.RPNCalculator.GetResult(""));

            Assert.ThrowsException<DivideByZeroException>(() => RPN_Calculator.RPNCalculator.GetResult("0 8 root"));

            Assert.ThrowsException<DivideByZeroException>(() => RPN_Calculator.RPNCalculator.GetResult("-1 0 root"));

            Assert.ThrowsException<InvalidOperationException>(() => RPN_Calculator.RPNCalculator.GetResult("root"));

            Assert.ThrowsException<KeyNotFoundException>(() => RPN_Calculator.RPNCalculator.GetResult("1.1 0.0 log"));

            Assert.ThrowsException<InvalidOperationException>(() => RPN_Calculator.RPNCalculator.GetResult("log 0 9"));

            Assert.ThrowsException<InvalidOperationException>(() => RPN_Calculator.RPNCalculator.GetResult("log"));
        }
    }
}
