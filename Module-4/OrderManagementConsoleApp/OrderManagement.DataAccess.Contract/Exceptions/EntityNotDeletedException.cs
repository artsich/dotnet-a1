using System;

namespace OrderManagement.DataAccess.Exceptions
{
    public class EntityNotDeletedException : Exception
    {
        public EntityNotDeletedException(string msg) :
            base(msg)
        {
        }
    }
}
