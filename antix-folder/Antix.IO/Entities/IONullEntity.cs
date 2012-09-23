using System;
using Antix.IO.Entities.Base;

namespace Antix.IO.Entities
{
    public class IONullEntity :
        IOEntity
    {
        protected IONullEntity(Parameters parameters)
            : base(parameters)
        {
        }

        public static IONullEntity Create(Action<Parameters> assign)
        {
            var p = new Parameters();
            assign(p);

            return new IONullEntity(p);
        }
    }
}