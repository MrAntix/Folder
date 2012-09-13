using Antix.IO.Events.Base;
using Antix.IO.Entities.Base;

namespace Antix.IO.Events
{
    public class IOCreatedEvent : IOEvent
    {
        public IOCreatedEvent(IOEntity entity) : base(entity)
        {
        }
    }
}