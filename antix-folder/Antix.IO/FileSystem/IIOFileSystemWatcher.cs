using System;
using Antix.IO.Entities.Base;
using Antix.IO.Events.Base;

namespace Antix.IO.FileSystem
{
    public interface IIOFileSystemWatcher
    {
        IObservable<IOEvent> Watch(IOEntity entity, IOWatchSettings settings);
    }
}