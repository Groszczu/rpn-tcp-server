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

            Assert.ThrowsException<DivideByZeroException>(() => RPN_Calculator.RPNCalculator.GetResult("1.1 0.0 log"));

            Assert.ThrowsException<InvalidOperationException>(() => RPN_Calculator.RPNCalculator.GetResult("log 0 9"));

            Assert.ThrowsException<InvalidOperationException>(() => RPN_Calculator.RPNCalculator.GetResult("log"));

            var expectedResult = 7.0d;
            var delta = 0.0001;
            var result = RPN_Calculator.RPNCalculator.GetResult("3 4 +");
            Assert.AreEqual(expectedResult, result, delta);

            expectedResult = -1.9d;
            result = RPN_Calculator.RPNCalculator.GetResult("2.1 4 -");
            Assert.AreEqual(expectedResult, result, delta);

            expectedResult = -4.4d;
            result = RPN_Calculator.RPNCalculator.GetResult("-2.2 2 *");
            Assert.AreEqual(expectedResult, result, delta);

            expectedResult = 4.0d;
            result = RPN_Calculator.RPNCalculator.GetResult("4.4 1.1 /");
            Assert.AreEqual(expectedResult, result, delta);

            expectedResult = 9.0d;
            result = RPN_Calculator.RPNCalculator.GetResult("3 2 ^");
            Assert.AreEqual(expectedResult, result, delta);

            expectedResult = 1.9d;
            result = RPN_Calculator.RPNCalculator.GetResult("5.1 3.2 %");
            Assert.AreEqual(expectedResult, result, delta);

            expectedResult = 4.0d;
            result = RPN_Calculator.RPNCalculator.GetResult("3 64 root");
            Assert.AreEqual(expectedResult, result, delta);

            expectedResult = 1.1132827526d;
            result = RPN_Calculator.RPNCalculator.GetResult("5 6 log");
            Assert.AreEqual(expectedResult, result, delta);
        }
    }
}
