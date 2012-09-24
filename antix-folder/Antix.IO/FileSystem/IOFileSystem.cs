using System;
using System.Collections.Generic;
using System.Linq;
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

        IEnumerable<IOCategoryEntity> IIOSystem.GetAncestors(IOEntity entity)
        {
            var parentDirectories = _fileSystemInfoProvider
                    .GetParentDirectories(entity.Identifier);
            return
                from directory in parentDirectories
                select IOCategoryEntity.Create(directory);
        }

        IEnumerable<IOCategoryEntity> IIOSystem.GetParents(IOEntity entity)
        {
            return new[]
                       {
                           IOCategoryEntity
                               .Create(_fileSystemInfoProvider
                                                .GetParentDirectory(entity.Identifier))
                       };
        }

        IEnumerable<IOEntity> IIOSystem.GetChildren(IOCategoryEntity entity)
        {
            return
                _fileSystemInfoProvider
                    .GetChildDirectoriesAndFiles(entity.Identifier)
                    .Select(_fileSystemInfoProvider.GetEntity);
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