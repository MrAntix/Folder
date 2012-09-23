using System;
using Antix.IO.Entities.Base;

namespace Antix.IO.Entities
{
    public class IOCategoryEntity :
        IOEntity
    {
        protected IOCategoryEntity(Parameters parameters)
            : base(parameters)
        {
        }

        public static IOCategoryEntity Create(Action<Parameters> assign)
        {
            var p = new Parameters();
            assign(p);

            return new IOCategoryEntity(p);
        }
    }
}