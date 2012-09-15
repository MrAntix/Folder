using Antix.IO.Entities.Base;
using Antix.IO.Events.Base;

namespace Antix.IO.Events
{
    public class IOCreatedEvent : IOEvent
    {
        public IOCreatedEvent(IOEntity entity) : base(entity)
        {
        }
    }
}