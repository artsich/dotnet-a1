using System;
using System.Collections.Generic;
using System.Text;

namespace Di.Exceptions
{
    public class ResolveDependencyException : Exception
    {
        public ResolveDependencyException(string message) : 
            base(message)
        {
        }
    }
}
