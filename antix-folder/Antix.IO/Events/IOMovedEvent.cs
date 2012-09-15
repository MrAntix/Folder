using Antix.IO.Entities.Base;
using Antix.IO.Events.Base;

namespace Antix.IO.Events
{
    public class IOMovedEvent : IOEvent
    {
        readonly IOEntity _newEntity;

        public IOMovedEvent(
            IOEntity entity, IOEntity newEntity) :
                base(entity)
        {
            _newEntity = newEntity;
        }

        public IOEntity NewEntity
        {
            get { return _newEntity; }
        }
    }
}