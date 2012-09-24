using Antix.IO.Entities.Base;

namespace Antix.IO.Entities
{
    public class IOCategoryEntity :
        IOEntity
    {
        protected IOCategoryEntity(string identifier)
            : base(identifier)
        {
        }

        public static IOCategoryEntity Create(string identifier)
        {
            return new IOCategoryEntity(identifier);
        }
    }
}