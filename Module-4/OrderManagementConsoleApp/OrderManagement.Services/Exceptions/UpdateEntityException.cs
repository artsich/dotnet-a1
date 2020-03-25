using System;

namespace OrderManagement.Services.Exceptions
{
    public class UpdateEntityException : Exception
    {
        public UpdateEntityException(string msg) :
            base(msg)
        {
        }
    }
}
