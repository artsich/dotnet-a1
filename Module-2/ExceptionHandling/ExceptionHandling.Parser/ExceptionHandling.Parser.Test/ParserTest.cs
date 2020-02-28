using ExceptionHandling.Parser.Exceptions;
using NUnit.Framework;
using System;

namespace ExceptionHandling.Parser.Test
{
    public class ParserTest
    {
        [Theory]
        [TestCase("3131", 3131)]
        [TestCase("0xABCDEF", 0xABCDEF)]
        [TestCase("0b111100011", 0b111100011)]
        [TestCase("-3131", -3131)]
        [TestCase("-0xABCDEF", -0xABCDEF)]
        [TestCase("-0b111100011", -0b111100011)]
        [TestCase("-0xabcdef", -0xabcdef)]
        public void IntParse_CheckParsingForEachNumberFormat_ReturnCorrectResult(string str, int expected)
        {
            var result = ParserUtil.IntParse(str);
            Assert.AreEqual(result, expected);
        }

        [Theory]
        [TestCase("3131", 3131)]
        [TestCase("0xABCDEF", 0xABCDEF)]
        [TestCase("0b111100011", 0b111100011)]

        public void TryParse_CheckParsigForEachNumberFormat_ReturnCorrectResult(string str, int expected)
        {
            bool parsed = ParserUtil.IntTryParse(str, out var result);
            Assert.IsTrue(parsed);
            Assert.AreEqual(result, expected);
        }

        [Theory]
        [TestCase("ggsgds")]
        [TestCase("-sdasd")]
        [TestCase("-0xsdasd")]
        [TestCase("-0bsdasd")]
        [TestCase("1283dsad2914")]

        public void TryPase_CheckIncorrectParsing_ReturnFalse(string str)
        {
            bool result = ParserUtil.IntTryParse(str, out _);
            Assert.IsFalse(result);
        }

        [Theory]
        [TestCase("11111112111")]
        [TestCase("0xfffffffff")]
        [TestCase("0b111111111111111111111111111111111")]
        public void IntParse_OutOfLengthRange_ReturnException(string str)
        {
            Assert.Throws<LenghtRangeException>(() => ParserUtil.IntParse(str));
        }

        [Theory]
        [TestCase("0b")]
        [TestCase("0x")]
        public void IntParse_AfterPrefixShoudBeDigits_ReturnInvalidFormatException(string str)
        {
            Assert.Throws<FormatException>(() => ParserUtil.IntParse(str));
        }

        [Theory]
        [TestCase("13adsad")]
        [TestCase("13a-dsad")]
        [TestCase("0xlllkasd")]
        [TestCase("0bfff111")]
        public void IntParse_UnexcpectedSymbol_ReturnInvalidFormatException(string str)
        {
            Assert.Throws<FormatException>(() => ParserUtil.IntParse(str));
        }

        [Theory]
        [TestCase("-")]
        public void IntParse_AfterDigitMustBeDigit_ReturnIncorrect(string str)
        {
            Assert.Throws<FormatException>(() => ParserUtil.IntParse(str));
        }

    }
}