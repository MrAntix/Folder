using System.Collections.Generic;
using Antix.IO.Entities;
using Antix.IO.Entities.Base;

namespace Antix.IO.FileSystem
{
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
        IOEntity GetEntity(string path);

        /// <summary>
        ///   <para> Get file entity </para>
        /// </summary>
        /// <param name="path"> path </param>
        /// <returns> Entity </returns>
        IOFileEntity GetFileEntity(string path);

        /// <summary>
        ///   <para> Get category entity </para>
        /// </summary>
        /// <param name="path"> path </param>
        /// <returns> Entity </returns>
        IOCategoryEntity GetCategoryEntity(string path);

        /// <summary>
        ///   <para> Get null entity </para>
        /// </summary>
        /// <param name="path"> path </param>
        /// <returns> Entity </returns>
        IONullEntity GetNullEntity(string path);
    }
}