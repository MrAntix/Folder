using System.Collections.Generic;
using Antix.IO.Entities;
using Antix.IO.Entities.Base;

namespace Antix.IO.FileSystem
{
    public interface IIOFileSystemInfoProvider
    {
        /// <summary>
        ///   <para> Get the parent categories for a given entity </para>
        /// </summary>
        /// <param name="entity"> Entity </param>
        /// <returns> Parent Categories </returns>
        IOCategoryEntity GetParentDirectory(IOEntity entity);

        /// <summary>
        ///   <para> Get the child entities for a given entity </para>
        /// </summary>
        /// <param name="entity"> Category Entity </param>
        /// <returns> Child entities </returns>
        IEnumerable<IOEntity> GetChildDirectoriesAndFiles(IOCategoryEntity entity);

        /// <summary>
        ///   <para> Get Entity </para>
        /// </summary>
        /// <param name="path"> path </param>
        /// <returns> Entity </returns>
        IOEntity GetEntity(string path);
    }
}