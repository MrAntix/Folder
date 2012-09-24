using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        async Task<IEnumerable<IOCategoryEntity>> IIOSystem.GetAncestorsAsync(IOEntity entity)
        {
            var parentDirectories = _fileSystemInfoProvider
                    .GetParentDirectories(entity.Identifier);
            return await Task.FromResult(
                from directory in parentDirectories
                select IOCategoryEntity.Create(directory));
        }

        async Task<IEnumerable<IOCategoryEntity>> IIOSystem.GetParentsAsync(IOEntity entity)
        {
            return await Task.FromResult(
                new[]
                       {
                           IOCategoryEntity
                               .Create(_fileSystemInfoProvider
                                                .GetParentDirectory(entity.Identifier))
                       });
        }

        async Task<IEnumerable<IOEntity>> IIOSystem.GetChildrenAsync(IOCategoryEntity entity)
        {
            return
                await Task.FromResult(
                    _fileSystemInfoProvider
                        .GetChildDirectoriesAndFiles(entity.Identifier)
                        .Select(_fileSystemInfoProvider.GetEntity)
                          );
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