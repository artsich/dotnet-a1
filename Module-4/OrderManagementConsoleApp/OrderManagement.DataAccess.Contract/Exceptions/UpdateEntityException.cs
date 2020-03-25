using System;

namespace OrderManagement.DataAccess.Exceptions
{
    public class UpdateEntityException : Exception
    {
        public UpdateEntityException(string msg) :
            base(msg)
        {
        }
    }
}
