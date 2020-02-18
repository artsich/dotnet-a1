using System;

namespace HelloCore
{
    public static class Formatter
    {
        public static string FormatToDateHello(string name)
        {
            return $"{DateTime.Now} Hello, {name}!";
        }
    }
}
