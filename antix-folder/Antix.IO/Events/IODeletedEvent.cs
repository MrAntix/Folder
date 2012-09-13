using Antix.IO.Events.Base;
using Antix.IO.Entities.Base;

namespace Antix.IO.Events
{
    public class IODeletedEvent : IOEvent
    {
        public IODeletedEvent(IOEntity entity) : base(entity)
        {
        }
    }
}