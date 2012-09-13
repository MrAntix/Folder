using Antix.IO.Entities.Base;

namespace Antix.IO.Events.Base
{
    public abstract class IOEvent
    {
        readonly IOEntity _entity;

        protected IOEvent(IOEntity entity)
        {
            _entity = entity;
        }
        public IOEntity Entity
        {
            get { return _entity; }
        }
    }
}