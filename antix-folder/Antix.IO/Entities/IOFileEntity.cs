using Antix.IO.Entities.Base;

namespace Antix.IO.Entities
{
    public class IOFileEntity :
        IOEntity
    {
        protected IOFileEntity(string identifier)
            : base(identifier)
        {
        }

        public static IOFileEntity Create(string identifier)
        {
            return new IOFileEntity(identifier);
        }
    }
}