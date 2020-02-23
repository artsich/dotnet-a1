using System;

namespace CoreHello
{
    class Program
    {
        static void Main(string[] args)
        {
            var hello = args.Length > 0 ? HelloCore.Formatter.FormatToDateHello(args[0]) : "Yyyyps, args[0] - is null";
            Console.WriteLine(hello);
        }
    }
}
