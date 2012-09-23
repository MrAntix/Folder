using System;
using System.Collections.Generic;
using Antix.IO.Entities;
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
        /// <para>Get the parent categories for a given entity</para>
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>Parent Categories</returns>
        IEnumerable<IOCategoryEntity> GetParentCategories(IOEntity entity);

        /// <summary>
        /// <para>Get the child entities for a given entity</para>
        /// </summary>
        /// <param name="entity">Category Entity</param>
        /// <returns>Child entities</returns>
        IEnumerable<IOEntity> GetChildEntities(IOCategoryEntity entity);

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