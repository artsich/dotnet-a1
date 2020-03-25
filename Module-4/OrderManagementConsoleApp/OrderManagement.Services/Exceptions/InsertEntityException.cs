using System;

namespace OrderManagement.Services.Exceptions
{
    public class InsertEntityException : Exception
    {
        public InsertEntityException()
        {

        }

        public InsertEntityException(string msg) :
            base(msg)
        {
        }
    }
}
