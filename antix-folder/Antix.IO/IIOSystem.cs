using System;
using Antix.IO.Entities.Base;
using Antix.IO.Events.Base;

namespace Antix.IO
{
    public interface IIOSystem
    {
        //Task<Stream> OpenReadAsync(IOFileEntity file);
        //Task<Stream> OpenWriteAsync(IOFileEntity file);

        //Task Copy(IOFileEntity file, IOFileEntity fileTo);

        IOEntity GetInfo(string path);

        IObservable<IOEvent> Watch(IOEntity entity);
        IObservable<IOEvent> Watch(IOEntity entity, IIOWatchSettings settings);
    }
}