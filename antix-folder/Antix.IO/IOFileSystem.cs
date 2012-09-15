using System;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using Antix.IO.Entities;
using Antix.IO.Entities.Base;
using Antix.IO.Events;
using Antix.IO.Events.Base;

namespace Antix.IO
{
    public class IOFileSystem : IIOSystem
    {
        IOEntity IIOSystem.GetInfo(string path)
        {
            return GetInfo(path);
        }

        public static IOEntity GetInfo(string path)
        {
            if (string.IsNullOrWhiteSpace(path)) throw new ArgumentException("path");

            return File.Exists(path)
                       ? new IOFileEntity {Path = path}
                       : Directory.Exists(path)
                             ? (IOEntity) new IODirectoryEntity {Path = path}
                             : new IONullEntity {Path = path};
        }

        IObservable<IOEvent> IIOSystem.Watch(IOEntity entity)
        {
            return Watch(entity, new WatchSettings());
        }

        IObservable<IOEvent> IIOSystem.Watch(IOEntity entity, WatchSettings settings)
        {
            return Watch(entity, settings);
        }

        IObservable<IOEvent> Watch(IOEntity entity, WatchSettings settings)
        {
            if (entity == null) throw new ArgumentNullException("entity");
            if (settings == null) throw new ArgumentNullException("settings");

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
                                                                   GetInfo(path)));
                                                       }
                                                       else
                                                       {
                                                           if (firstRenamed != null)
                                                           {
                                                               // raise the rename on the old file
                                                               observer.OnNext(
                                                                   new IOMovedEvent(
                                                                       GetInfo(firstRenamed.OldFullPath),
                                                                       GetInfo(firstRenamed.FullPath)));
                                                           }

                                                           if (first.ChangeType == WatcherChangeTypes.Created)
                                                           {
                                                               observer.OnNext(
                                                                   new IOCreatedEvent(
                                                                       GetInfo(last.FullPath)));
                                                           }
                                                           else
                                                           {
                                                               observer.OnNext(
                                                                   new IOUpdatedEvent(
                                                                       GetInfo(first.FullPath))
                                                                   );
                                                           }
                                                       }
                                                   }
                                               });

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