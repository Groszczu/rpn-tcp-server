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
        private static readonly Dictionary<char, Func<double, double, double>> _signToOperation = new Dictionary<char, Func<double, double, double>>()
        {
            { '-', (a, b) => a - b },
            { '+', (a, b) => a + b },
            { '*', (a, b) => a * b },
            { '/', (a, b) => a / b },
        };

        private const string InvalidInputMessage = "Input is not a valid RPN";

        /// <summary>
        /// Function that returns result of RPN input
        /// Throws Exception when input is not valid RPN
        /// Example: RPNCalculator.Calculate("1 2 + 5 *") -> 15
        /// </summary>
        /// <param name="input">RPN expression</param>
        /// <returns>result of RPN expression</returns>
        public static double Calculate(string input)
        {
            try
            {
                return GetResult(input);
            }
            catch (Exception)
            {
                throw new ArgumentException($"{InvalidInputMessage}. Input: {input}");
            }
        }

        private static double GetResult(string input)
        {
            var valuesStack = new Stack<double>();
            Array.ForEach(input.Split(' '), n =>
            {
                if (double.TryParse(n, out var num))
                {
                    valuesStack.Push(num);
                }
                else
                {
                    var func = _signToOperation[n[0]];

                    var arg2 = valuesStack.Pop();
                    var arg1 = valuesStack.Pop();
                    var result = func(arg1, arg2);
                    valuesStack.Push(result);
                }
            });

            if (valuesStack.Count() != 1)
            {
                throw new ArgumentException(InvalidInputMessage);
            }

            return valuesStack.Pop();
        }
    }
}

