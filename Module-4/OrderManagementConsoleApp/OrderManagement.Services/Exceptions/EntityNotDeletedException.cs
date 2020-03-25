using System;

namespace OrderManagement.Services.Exceptions
{
    public class EntityNotDeletedException : Exception
    {
        public EntityNotDeletedException(string msg) :
            base(msg)
        {
        }
    }
}
