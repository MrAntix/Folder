using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive;
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
            IOEntity entity, IOWatchSettings settings)
        {
            if (entity == null) throw new ArgumentNullException("entity");

            return Observable
                .Create<IOEvent>(
                    observer
                    =>
                        {
                            var fileSystemWatcher = new FileSystemWatcher(entity.Identifier)
                                                        {
                                                            EnableRaisingEvents = true,
                                                            IncludeSubdirectories = settings.IncludeSubCategories
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
                                                                   _fileSystemInfoProvider.GetEntity<IOEntity>(path)));
                                                       }
                                                       else
                                                       {
                                                           if (firstRenamed != null)
                                                           {
                                                               // raise the rename on the old file
                                                               observer.OnNext(
                                                                   new IOMovedEvent(
                                                                       _fileSystemInfoProvider.GetEntity<IOEntity>(firstRenamed.OldFullPath),
                                                                       _fileSystemInfoProvider.GetEntity<IOEntity>(firstRenamed.FullPath)));
                                                           }

                                                           if (first.ChangeType == WatcherChangeTypes.Created)
                                                           {
                                                               observer.OnNext(
                                                                   new IOCreatedEvent(
                                                                       _fileSystemInfoProvider.GetEntity<IOEntity>(last.FullPath)));
                                                           }
                                                           else
                                                           {
                                                               observer.OnNext(
                                                                   new IOUpdatedEvent(
                                                                       _fileSystemInfoProvider.GetEntity<IOEntity>(first.FullPath))
                                                                   );
                                                           }
                                                       }
                                                   }
                                               },
                                           observer.OnError);

                            return fileSystemWatcher.Dispose;
                        });
        }

        IObservable<FileSystemEventArgs> GetFileSystemWatcherObservables(
            FileSystemWatcher fileSystemWatcher)
        {
            if (fileSystemWatcher == null) throw new ArgumentNullException("fileSystemWatcher");

            return new[]
                       {
                           Observable
                               .FromEventPattern<FileSystemEventArgs>(fileSystemWatcher, "Created")
                               .SelectMany(
                                   evp =>
                                   GetDirectoryChangedEventArgs(evp, fileSystemWatcher.IncludeSubdirectories,
                                                                fileSystemWatcher.Path)),
                           Observable
                               .FromEventPattern<FileSystemEventArgs>(fileSystemWatcher, "Changed")
                               .SelectMany(
                                   evp =>
                                   GetDirectoryChangedEventArgs(evp, fileSystemWatcher.IncludeSubdirectories,
                                                                fileSystemWatcher.Path)),
                           Observable
                               .FromEventPattern<RenamedEventArgs>(fileSystemWatcher, "Renamed")
                               .SelectMany(
                                   evp =>
                                   GetDirectoryChangedEventArgs(evp, fileSystemWatcher.IncludeSubdirectories,
                                                                fileSystemWatcher.Path)),
                           Observable
                               .FromEventPattern<FileSystemEventArgs>(fileSystemWatcher, "Deleted")
                               .SelectMany(
                                   evp =>
                                   GetDirectoryChangedEventArgs(evp, fileSystemWatcher.IncludeSubdirectories,
                                                                fileSystemWatcher.Path)),
                           Observable
                               .FromEventPattern<ErrorEventArgs>(fileSystemWatcher, "Error")
                               .SelectMany(ev => Observable.Throw<FileSystemEventArgs>(ev.EventArgs.GetException()))
                       }
                .Merge();
        }


        IEnumerable<FileSystemEventArgs> GetDirectoryChangedEventArgs(
            EventPattern<FileSystemEventArgs> eventPattern, bool bubble, string rootPath)
        {
            yield return eventPattern.EventArgs;

            if (!bubble) yield break;

            // bubble to parents
            foreach (var directory in  _fileSystemInfoProvider
                .GetParentDirectories(eventPattern.EventArgs.FullPath))
            {
                var parentDirectory = Path.GetDirectoryName(directory);
                if (parentDirectory != null)
                    yield return new FileSystemEventArgs(
                        WatcherChangeTypes.Changed,
                        parentDirectory,
                        Path.GetFileName(directory) + "\\");

                if (rootPath.Length > directory.Length) yield break;
            }
        }

        IEnumerable<FileSystemEventArgs> GetDirectoryChangedEventArgs(
            EventPattern<RenamedEventArgs> eventPattern, bool bubble, string rootPath)
        {
            return GetDirectoryChangedEventArgs(
                new EventPattern<FileSystemEventArgs>(eventPattern.Sender, eventPattern.EventArgs),
                bubble, rootPath);

            // bubble to parents
        }
    }
}