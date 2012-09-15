using Antix.IO.Entities.Base;
using Antix.IO.Events.Base;

namespace Antix.IO.Events
{
    public class IOUpdatedEvent : IOEvent
    {
        public IOUpdatedEvent(IOEntity entity) : base(entity)
        {
        }
    }
}