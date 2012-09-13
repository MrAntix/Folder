using Antix.IO.Events.Base;
using Antix.IO.Entities.Base;

namespace Antix.IO.Events
{
    public class IOUpdatedEvent : IOEvent {
        public IOUpdatedEvent(IOEntity entity) : base(entity){}
    }
}