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

        /// <summary>
        /// <para>Get an entity in the IO System based on an appropriate identifier</para>
        /// </summary>
        /// <param name="identifier">Identifier</param>
        /// <returns>IO entiy</returns>
        IOEntity GetEntity(string identifier);

        /// <summary>
        /// <para>Watch for changes on the entity passed</para>
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>Observable Events</returns>
        IObservable<IOEvent> Watch(IOEntity entity);

        /// <summary>
        /// <para>Watch for changes on the entity passed</para>
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <param name="settings">Settings for the watch</param>
        /// <returns>Observable Events</returns>
        IObservable<IOEvent> Watch(IOEntity entity, IOWatchSettings settings);
    }
}