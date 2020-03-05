using System;

namespace Di.Exceptions
{
    public class MultiplyImplementingException : Exception
    {
        public MultiplyImplementingException(string message) : 
            base(message)
        {

        }
    }
}
