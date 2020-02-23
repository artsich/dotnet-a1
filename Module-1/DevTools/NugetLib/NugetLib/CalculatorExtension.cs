using System;
using System.Collections.Generic;
using System.Text;

namespace NugetLib
{
    public static class CalculatorExtension
    {
        public static int Sum(this int left, int right)
        {
            return left + right;
        }
    }
}
