using System;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using Antix.IO.Events;
using Antix.IO.Events.Base;
using Antix.IO.Entities;
using Antix.IO.Entities.Base;

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
            return File.Exists(path)
                       ? (IOEntity) new IOFileEntity {Path = path}
                       : new IODirectoryEntity {Path = path};
        }

        IObservable<IOEvent> IIOSystem.Watch(IOEntity entity)
        {
            return Watch(entity, TimeSpan.FromSeconds(2));
        }

        IObservable<IOEvent> IIOSystem.Watch(IOEntity entity, TimeSpan interval)
        {
            return Watch(entity, interval);
        }

        IObservable<IOEvent> Watch(IOEntity entity, TimeSpan interval)
        {
            return Observable
                .Create<IOEvent>(
                    observer
                    =>
                        {
                            var fileSystemWatcher = new FileSystemWatcher(entity.Path)
                                                        {
                                                            EnableRaisingEvents = true,
                                                            IncludeSubdirectories = true
                                                        };

                            GetFileSystemWatcherObservables(fileSystemWatcher)
                                .Buffer(interval)
                                .Subscribe(ae =>
                                               {
                                                   foreach (var es in ae.GroupBy(x => x.Entity.Path))
                                                   {
                                                       var last = es.Last();
                                                       if (last is IODeletedEvent)
                                                           observer.OnNext(last);

                                                       else
                                                       {
                                                           var first = es.First();
                                                           if (first is IOCreatedEvent)
                                                               observer.OnNext(first);

                                                           else
                                                           {
                                                               observer.OnNext(
                                                                   new IOUpdatedEvent(first.Entity)
                                                                   );
                                                           }
                                                       }
                                                   }
                                               });

                            return fileSystemWatcher.Dispose;
                        });
        }

        IObservable<IOEvent> GetFileSystemWatcherObservables(FileSystemWatcher fileSystemWatcher)
        {
            return new[]
                       {
                           Observable
                               .FromEventPattern<FileSystemEventArgs>(fileSystemWatcher, "Created")
                               .Select(ev => new IOCreatedEvent(GetInfo(ev.EventArgs.FullPath))),

                           Observable
                               .FromEventPattern<FileSystemEventArgs>(fileSystemWatcher, "Changed")
                               .Select(ev => new IOUpdatedEvent(GetInfo(ev.EventArgs.FullPath))),

                           Observable
                               .FromEventPattern<RenamedEventArgs>(fileSystemWatcher, "Renamed")
                               .Select(ev =>
                                       new IORenamedEvent(
                                           GetInfo(ev.EventArgs.FullPath),
                                           ev.EventArgs.OldFullPath
                                           )
                               ),

                           Observable
                               .FromEventPattern<FileSystemEventArgs>(fileSystemWatcher, "Deleted")
                               .Select(ev => new IODeletedEvent(GetInfo(ev.EventArgs.FullPath))),

                           Observable
                               .FromEventPattern<ErrorEventArgs>(fileSystemWatcher, "Error")
                               .SelectMany(ev => Observable.Throw<IOEvent>(ev.EventArgs.GetException()))
                       }
                .Merge();
        }
    }
}