using System;

namespace ExceptionHandling.Parser.Exceptions
{
    public class LenghtRangeException : Exception
    {
        public LenghtRangeException(string message) :
            base(message)
        {
        }
    }
}
