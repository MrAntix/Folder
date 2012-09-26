using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antix.IO.Entities;
using Antix.IO.Entities.Base;
using Antix.IO.Events.Base;

namespace Antix.IO.FileSystem
{
    public class IOFileSystem : IIOSystem
    {
        readonly IIOFileSystemWatcher _watcher;
        readonly IIOFileSystemInfoProvider _infoProvider;
        readonly IIOFileSystemWorker _worker;

        public IOFileSystem(
            IIOFileSystemWatcher watcher, 
            IIOFileSystemInfoProvider infoProvider, 
            IIOFileSystemWorker worker)
        {
            _watcher = watcher;
            _infoProvider = infoProvider;
            _worker = worker;
        }

        IOEntity IIOSystem.GetEntity(string identifier)
        {
            return _infoProvider.GetEntity(identifier);
        }

        async Task<IEnumerable<IOCategoryEntity>> IIOSystem.GetAncestorsAsync(IOEntity entity)
        {
            var parentDirectories = _infoProvider
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
                               .Create(_infoProvider
                                                .GetParentDirectory(entity.Identifier))
                       });
        }

        async Task<IEnumerable<IOEntity>> IIOSystem.GetChildrenAsync(IOCategoryEntity entity)
        {
            return
                await Task.FromResult(
                    _infoProvider
                        .GetChildDirectoriesAndFiles(entity.Identifier)
                        .Select(_infoProvider.GetEntity)
                          );
        }

        IObservable<IOEvent> IIOSystem.Watch(IOEntity entity, IOWatchSettings settings)
        {
            return _watcher.Watch(entity, settings);
        }

        IOFileEntityWriter IIOSystem.CreateFile(string identifier, Encoding encoding)
        {
            return IOFileEntityWriter.Create(
                _worker.CreateFile(identifier),
                encoding,
                IOFileEntity.Create(identifier)
                );
        }
    }
}