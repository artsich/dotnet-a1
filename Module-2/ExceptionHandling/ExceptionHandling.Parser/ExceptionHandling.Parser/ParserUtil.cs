using ExceptionHandling.Parser.Exceptions;
using System;
using System.Collections.Generic;

namespace ExceptionHandling.Parser
{
    public static class ParserUtil
    {
        private static int MaxStringIntegerLength = int.MaxValue.ToString().Length;
        private static int MaxStringIntegerHexLength = 8;
        private static int MaxStringIntegerBinaryLength = 32;

        private static Dictionary<char, int> FromCharToBinaryDigitMap = new Dictionary<char, int>
        {
            {'0', 0},
            {'1', 1},
        };

        private static Dictionary<char, int> FromCharToDigitMap = new Dictionary<char, int>(FromCharToBinaryDigitMap)
        {
            {'2', 2},
            {'3', 3},
            {'4', 4},
            {'5', 5},
            {'6', 6},
            {'7', 7},
            {'8', 8},
            {'9', 9},
        };

        private static Dictionary<char, int> FromCharToDigitMapHex = new Dictionary<char, int>(FromCharToDigitMap) {
            {'A', 10}, {'a', 10},
            {'B', 11}, {'b', 11},
            {'C', 12}, {'c', 12},
            {'D', 13}, {'d', 13},
            {'E', 14}, {'e', 14},
            {'F', 15}, {'f', 15},
        };

        public static bool IntTryParse(string str, out int intResult)
        {
            var boolResult = true;
            intResult = 0;
            try
            {
                intResult = IntParse(str);
            }
            catch (Exception)
            {
                boolResult = false;
            }

            return boolResult;
        }

        public static int IntParse(string str)
        {
            if (str.Length == 0)
            {
                throw new FormatException("Length of string should be more than zero....");
            }

            var isMinus = str[0] == '-';

            if (isMinus)
            {
                if (str.Length < 2)
                {
                    throw new FormatException("After minus should be digits");
                }

                str = str.Substring(1, str.Length - 1);
            }

            var digitBase = 10;
            var digitMap = FromCharToDigitMap;
            var startIndex = 0;

            if (str.Length >= 2)
            {
                if (str[0] == '0')
                {
                    if (str[1] == 'x')
                    {
                        digitBase = 16;
                        startIndex = 2;
                        digitMap = FromCharToDigitMapHex;
                        if (str.Length < 2 || str.Length - 2 > MaxStringIntegerHexLength)
                        {
                            throw new LenghtRangeException($"Hex length must be less than {MaxStringIntegerHexLength}");
                        }
                    }
                    else if (str[1] == 'b')
                    {
                        digitBase = 2;
                        startIndex = 2;
                        digitMap = FromCharToBinaryDigitMap;

                        if (str.Length < 2 || str.Length - 2 > MaxStringIntegerBinaryLength)
                        {
                            throw new LenghtRangeException($"Binary length must be less than {MaxStringIntegerBinaryLength}");
                        }
                    }
                }
            }

            if (digitBase == 10 && str.Length > MaxStringIntegerLength)
            {
                throw new LenghtRangeException($"Decimal length must be less than {MaxStringIntegerLength}");

            }

            if (startIndex >= str.Length)
            {
                throw new FormatException("After prefix for HEX or Binary format must be digits..");
            }

            long result = 0;

            foreach (var ch in str.Substring(startIndex))
            {
                if (!digitMap.TryGetValue(ch, out var val))
                {
                    throw new FormatException($"Unexpected symbol '{ch}'");
                }

                result = (result * digitBase) + val;
            }
            if (result > int.MaxValue)
            {
                throw new LenghtRangeException($"Decimal length must be less than {MaxStringIntegerLength}");
            }

            return isMinus ? (int)-result : (int)result;
        }
    }
}
