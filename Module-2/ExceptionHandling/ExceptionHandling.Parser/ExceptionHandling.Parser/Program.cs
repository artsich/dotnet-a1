using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq.Expressions;

namespace ExceptionHandling.Parser
{
    class Program
    {
        static void Main(string[] args)
        {
            TestCase("1234");
            TestCase("0x231313");
            TestCase("0b101001");
        }

        static void TestCase(string str)
        {
            var defaultParser = int.TryParse(str, out var val) ? val.ToString() : "yyps, stupid default parser";
            Console.WriteLine(
                $"{ParserUtil.IntParse(str)} === {defaultParser}");
        }
    }
}
