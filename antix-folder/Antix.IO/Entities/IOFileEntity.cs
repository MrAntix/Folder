using System;
using Antix.IO.Entities.Base;

namespace Antix.IO.Entities
{
    public class IOFileEntity :
        IOEntity
    {
        protected IOFileEntity(Parameters parameters)
            : base(parameters)
        {
        }

        public static IOFileEntity Create(Action<Parameters> assign)
        {
            var p = new Parameters();
            assign(p);

            return new IOFileEntity(p);
        }
    }
}