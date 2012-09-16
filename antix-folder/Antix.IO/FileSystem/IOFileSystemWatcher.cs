using System;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using Antix.IO.Entities.Base;
using Antix.IO.Events;
using Antix.IO.Events.Base;

namespace Antix.IO.FileSystem
{
    public class IOFileSystemWatcher :
        IIOFileSystemWatcher
    {
        readonly IIOFileSystemInfoProvider _fileSystemInfoProvider;

        public IOFileSystemWatcher(
            IIOFileSystemInfoProvider fileSystemInfoProvider)
        {
            _fileSystemInfoProvider = fileSystemInfoProvider;
        }

        public IObservable<IOEvent> Watch(
            IOEntity entity, IIOWatchSettings settings)
        {
            if (entity == null) throw new ArgumentNullException("entity");

            return Observable
                .Create<IOEvent>(
                    observer
                    =>
                        {
                            var fileSystemWatcher = new FileSystemWatcher(entity.Path)
                                                        {
                                                            EnableRaisingEvents = true,
                                                            IncludeSubdirectories = settings.IncludeSubdirectories
                                                        };

                            GetFileSystemWatcherObservables(fileSystemWatcher)
                                .Buffer(settings.Interval)
                                .Subscribe(ae =>
                                               {
                                                   foreach (var es in ae.GroupBy(x => x.FullPath))
                                                   {
                                                       var first = es.First();
                                                       var firstRenamed = es.First() as RenamedEventArgs;

                                                       var last = es.Last();
                                                       if (last.ChangeType == WatcherChangeTypes.Deleted)
                                                       {
                                                           // if deleted, check if renamed 
                                                           // and raise the old file as deleted
                                                           var path = firstRenamed == null
                                                                          ? last.FullPath
                                                                          : firstRenamed.OldFullPath;

                                                           observer.OnNext(
                                                               new IODeletedEvent(
                                                                   _fileSystemInfoProvider.GetInfo(path)));
                                                       }
                                                       else
                                                       {
                                                           if (firstRenamed != null)
                                                           {
                                                               // raise the rename on the old file
                                                               observer.OnNext(
                                                                   new IOMovedEvent(
                                                                       _fileSystemInfoProvider.GetInfo(
                                                                           firstRenamed.OldFullPath),
                                                                       _fileSystemInfoProvider.GetInfo(
                                                                           firstRenamed.FullPath)));
                                                           }

                                                           if (first.ChangeType == WatcherChangeTypes.Created)
                                                           {
                                                               observer.OnNext(
                                                                   new IOCreatedEvent(
                                                                       _fileSystemInfoProvider.GetInfo(last.FullPath)));
                                                           }
                                                           else
                                                           {
                                                               observer.OnNext(
                                                                   new IOUpdatedEvent(
                                                                       _fileSystemInfoProvider.GetInfo(first.FullPath))
                                                                   );
                                                           }
                                                       }
                                                   }
                                               },
                                           observer.OnError);

                            return fileSystemWatcher.Dispose;
                        });
        }

        IObservable<FileSystemEventArgs> GetFileSystemWatcherObservables(FileSystemWatcher fileSystemWatcher)
        {
            if (fileSystemWatcher == null) throw new ArgumentNullException("fileSystemWatcher");

            return new[]
                       {
                           Observable
                               .FromEventPattern<FileSystemEventArgs>(fileSystemWatcher, "Created")
                               .Select(ev => ev.EventArgs),
                           Observable
                               .FromEventPattern<FileSystemEventArgs>(fileSystemWatcher, "Changed")
                               .Select(ev => ev.EventArgs),
                           Observable
                               .FromEventPattern<RenamedEventArgs>(fileSystemWatcher, "Renamed")
                               .Select(ev => ev.EventArgs),
                           Observable
                               .FromEventPattern<FileSystemEventArgs>(fileSystemWatcher, "Deleted")
                               .Select(ev => ev.EventArgs),
                           Observable
                               .FromEventPattern<ErrorEventArgs>(fileSystemWatcher, "Error")
                               .SelectMany(ev => Observable.Throw<FileSystemEventArgs>(ev.EventArgs.GetException()))
                       }
                .Merge();
        }
    }
}