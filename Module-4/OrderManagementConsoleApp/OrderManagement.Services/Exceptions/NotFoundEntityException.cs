using System;

namespace OrderManagement.Services.Exceptions
{
    public class NotFoundEntityException : Exception
    {
        public NotFoundEntityException(string msg) : 
            base(msg)
        {
        }
    }
}
