using Antix.IO.Entities.Base;

namespace Antix.IO.Entities
{
    public class IONullEntity :
        IOEntity
    {
        protected IONullEntity(string identifier)
            : base(identifier)
        {
        }

        public static IONullEntity Create(string identifier)
        {
            return new IONullEntity(identifier);
        }
    }
}