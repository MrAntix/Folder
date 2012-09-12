using System;
using System.IO;
using System.Linq;
using System.Reactive.Linq;

namespace Antix.IO
{
    public interface IIOSystem
    {
        //Task<Stream> OpenReadAsync(IOFile file);
        //Task<Stream> OpenWriteAsync(IOFile file);

        //Task Copy(IOFile file, IOFile fileTo);

        IObservable<IOEvent> Watch(IODirectory directory);
        IObservable<IOEvent> Watch(IODirectory directory, TimeSpan interval);
    }

    public class IOFileSystem : IIOSystem
    {
        IOObject GetInfo(string path)
        {
            return File.Exists(path)
                       ? (IOObject) new IOFile {Path = path}
                       : new IODirectory {Path = path};
        }

        public IObservable<IOEvent> Watch(IODirectory directory)
        {
            return Watch(directory, TimeSpan.FromSeconds(2));
        }

        public IObservable<IOEvent> Watch(IODirectory directory, TimeSpan interval)
        {
            return Observable
                .Create<IOEvent>(
                    observer
                    =>
                        {
                            var fileSystemWatcher = new FileSystemWatcher(directory.Path)
                                                        {
                                                            EnableRaisingEvents = true,
                                                            IncludeSubdirectories = true
                                                        };

                            GetFileSystemWatcherObservables(fileSystemWatcher)
                                .Buffer(interval)
                                .Subscribe(ae =>
                                               {
                                                   foreach (var es in ae.GroupBy(x => x.Object.Path))
                                                   {
                                                       var last = es.Last();
                                                       if (last.Type == IOEventType.Deleted)
                                                           observer.OnNext(last);

                                                       else
                                                       {
                                                           var first = es.First();
                                                           if (first.Type == IOEventType.Created)
                                                               observer.OnNext(first);

                                                           else
                                                           {
                                                               first.Type = IOEventType.Updated;
                                                               observer.OnNext(first);
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
                               .Select(ev => new IOEvent
                                                 {
                                                     Object = GetInfo(ev.EventArgs.FullPath),
                                                     Type = IOEventType.Created
                                                 }),
                           Observable
                               .FromEventPattern<FileSystemEventArgs>(fileSystemWatcher, "Changed")
                               .Select(ev => new IOEvent
                                                 {
                                                     Object = GetInfo(ev.EventArgs.FullPath),
                                                     Type = IOEventType.Updated
                                                 }),
                           Observable
                               .FromEventPattern<RenamedEventArgs>(fileSystemWatcher, "Renamed")
                               .Select(ev => new IOEvent
                                                 {
                                                     Object = GetInfo(ev.EventArgs.FullPath),
                                                     Type = IOEventType.Updated
                                                 }),
                           Observable
                               .FromEventPattern<FileSystemEventArgs>(fileSystemWatcher, "Deleted")
                               .Select(ev => new IOEvent
                                                 {
                                                     Object = GetInfo(ev.EventArgs.FullPath),
                                                     Type = IOEventType.Deleted
                                                 }),
                           Observable
                               .FromEventPattern<ErrorEventArgs>(fileSystemWatcher, "Error")
                               .SelectMany(ev =>
                                           Observable.Throw<IOEvent>(ev.EventArgs.GetException()))
                       }
                .Merge();
        }
    }
}