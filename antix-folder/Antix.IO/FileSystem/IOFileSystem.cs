using System;
using Antix.IO.Entities.Base;
using Antix.IO.Events.Base;

namespace Antix.IO.FileSystem
{
    public class IOFileSystem : IIOSystem
    {
        readonly IIOFileSystemWatcher _watcher;
        readonly IIOFileSystemInfoProvider _fileSystemInfoProvider;

        public IOFileSystem(
            IIOFileSystemWatcher watcher,
            IIOFileSystemInfoProvider fileSystemInfoProvider)
        {
            _watcher = watcher;
            _fileSystemInfoProvider = fileSystemInfoProvider;
        }

        IOEntity IIOSystem.GetInfo(string path)
        {
            return _fileSystemInfoProvider.GetInfo(path);
        }

        IObservable<IOEvent> IIOSystem.Watch(IOEntity entity)
        {
            return _watcher.Watch(entity, new IOWatchSettings());
        }

        IObservable<IOEvent> IIOSystem.Watch(IOEntity entity, IOWatchSettings settings)
        {
            return _watcher.Watch(entity, settings);
        }
    }
}