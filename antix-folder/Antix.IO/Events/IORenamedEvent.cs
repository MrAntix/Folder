using Antix.IO.Events.Base;
using Antix.IO.Entities.Base;

namespace Antix.IO.Events
{
    public class IORenamedEvent : IOEvent
    {
        readonly string _oldPath;

        public IORenamedEvent(
            IOEntity entity, string oldPath) :
            base(entity)
        {
            _oldPath = oldPath;
        }

        public string OldPath
        {
            get { return _oldPath; }
        }
    }
}