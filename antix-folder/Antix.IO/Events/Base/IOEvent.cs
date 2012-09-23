using System;
using Antix.IO.Entities.Base;

namespace Antix.IO.Events.Base
{
    public abstract class IOEvent
    {
        readonly IOEntity _entity;

        protected IOEvent(IOEntity entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");

            _entity = entity;
        }

        public IOEntity Entity
        {
            get { return _entity; }
        }

        public override string ToString()
        {
            return string.Format("{0}: {1}", GetType().Name, Entity.Identifier);
        }
    }
}