using System;

namespace OrderManagement.DataAccess.Exceptions
{
    public class NotFoundEntityException : Exception
    {
        public NotFoundEntityException(string msg) : 
            base(msg)
        {
        }
    }
}
