using System.Collections.Generic;
using Antix.IO.Entities;
using Antix.IO.Entities.Base;

namespace Antix.IO.FileSystem
{
    /// <summary>
    /// <para>Provides information on entities in the system</para>
    /// </summary>
    public interface IIOFileSystemInfoProvider
    {
        /// <summary>
        ///   <para> Get the parent directory for a given entity </para>
        /// </summary>
        /// <param name="path"> Entity path </param>
        /// <returns> Parent directory </returns>
        string GetParentDirectory(string path);

        /// <summary>
        ///   <para> Get the parent directories for a given entity </para>
        /// </summary>
        /// <param name="path"> Entity path </param>
        /// <returns> Parent directories </returns>
        IEnumerable<string> GetParentDirectories(string path);

        /// <summary>
        ///   <para> Get the child entities for a given entity </para>
        /// </summary>
        /// <param name="path"> Category path </param>
        /// <returns> Child entities </returns>
        IEnumerable<string> GetChildDirectoriesAndFiles(string path);

        /// <summary>
        ///   <para> Get Entity </para>
        /// </summary>
        /// <param name="path"> path </param>
        /// <returns> Entity </returns>
        TEntity GetEntity<TEntity>(string path)
            where TEntity : IOEntity;
    }
}