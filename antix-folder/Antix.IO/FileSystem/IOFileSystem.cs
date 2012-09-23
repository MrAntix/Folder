using System;
using System.Collections.Generic;
using Antix.IO.Entities;
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

        IOEntity IIOSystem.GetEntity(string identifier)
        {
            return _fileSystemInfoProvider.GetEntity(identifier);
        }

        IEnumerable<IOCategoryEntity> IIOSystem.GetParentCategories(IOEntity entity)
        {
            return _fileSystemInfoProvider.GetParentCategories(entity);
        }

        IEnumerable<IOEntity> IIOSystem.GetChildEntities(IOCategoryEntity entity)
        {
            return _fileSystemInfoProvider.GetChildEntities(entity);
        }

        IObservable<IOEvent> IIOSystem.Watch(IOEntity entity)
        {
            return _watcher.Watch(entity, IOWatchSettings.Default);
        }

        IObservable<IOEvent> IIOSystem.Watch(IOEntity entity, IOWatchSettings settings)
        {
            return _watcher.Watch(entity, settings);
        }
    }
}