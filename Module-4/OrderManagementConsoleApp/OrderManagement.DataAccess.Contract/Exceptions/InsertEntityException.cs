using System;

namespace OrderManagement.DataAccess.Exceptions
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
