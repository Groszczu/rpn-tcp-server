using System;
using System.Collections.Generic;
using System.Linq;

namespace RPN_Calculator
{
    /// <summary>
    /// Static class to transform Reversed Polish Notation inputs into result
    /// </summary>
    public static class RPNCalculator
    {
        private static readonly Dictionary<string, Func<double, double, double>> _signToOperation = new Dictionary<string, Func<double, double, double>>()
        {
            { "-", (a, b) => a - b },
            { "+", (a, b) => a + b },
            { "*", (a, b) => a * b },
            { "/", (a, b) => a / b },
            { "^", (a, b) => (double)Math.Pow(a, b) },
            { "%", (a, b) => a % b},
            { "root", (a, b) => (double)Math.Pow(b, 1.0 / a)},
            { "log", (a, b) => (double)Math.Log(b, a)},
        };

        private const string InvalidInputMessage = "Input is not a valid RPN";

        /// <summary>
        /// Function that returns result of RPN input
        /// Throws Exception when input is not valid RPN
        /// Example: RPNCalculator.Calculate("1 2 + 5 *") -> 15
        /// </summary>
        /// <param name="input">RPN expression</param>
        /// <returns>result of RPN expression</returns>

        public static double GetResult(string input)
        {
            var valuesStack = new Stack<double>();

            Array.ForEach(input?.Split(' ') ?? throw new ArgumentException("empty input"), n =>
            {
                if (double.TryParse(n, out double num))
                {
                    valuesStack.Push(num);
                }
                else
                {
                    var func = _signToOperation[n];

                    var arg2 = valuesStack.Pop();
                    var arg1 = valuesStack.Pop();
                    var result = func(arg1, arg2);
                    if (double.IsNegativeInfinity(result) || double.IsPositiveInfinity(result))
                        throw new DivideByZeroException(InvalidInputMessage);
                    valuesStack.Push(result);
                }
            });

            if (valuesStack.Count() != 1)
            {
                throw new IndexOutOfRangeException(InvalidInputMessage);
            }

            return valuesStack.Pop();
        }
    }
}

