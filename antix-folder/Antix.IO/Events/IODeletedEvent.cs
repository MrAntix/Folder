using Antix.IO.Entities.Base;
using Antix.IO.Events.Base;

namespace Antix.IO.Events
{
    public class IODeletedEvent : IOEvent
    {
        public IODeletedEvent(IOEntity entity) : base(entity)
        {
        }
    }
}