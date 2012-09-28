using System;
using System.Collections.Generic;
using System.IO;
using Antix.IO.Entities;
using Antix.IO.Entities.Base;

namespace Antix.IO.FileSystem
{
    public class IOFileSystemInfoProvider : IIOFileSystemInfoProvider
    {
        string IIOFileSystemInfoProvider
            .GetParentDirectory(string path)
        {
            return GetParentDirectory(path);
        }

        IEnumerable<string> IIOFileSystemInfoProvider
            .GetParentDirectories(string path)
        {
            return GetParentDirectories(path);
        }

        IEnumerable<string> IIOFileSystemInfoProvider
            .GetChildDirectoriesAndFiles(string path)
        {
            foreach (var directoryPath in GetChildDirectories(path))
            {
                yield return directoryPath;
            }
            foreach (var filePath in GetChildFiles(path))
            {
                yield return filePath;
            }
        }

        TEntity IIOFileSystemInfoProvider
            .GetEntity<TEntity>(string path)
        {
            if (string.IsNullOrWhiteSpace(path)) throw new ArgumentException("identifier");

            return GetEntity<TEntity>(path);
        }

        // private routines

        string GetParentDirectory(string path)
        {
            return
                Path.GetDirectoryName(path);
        }

        IEnumerable<string> GetParentDirectories(string path)
        {
            var parent = path;
            while ((parent = Path.GetDirectoryName(parent)) != null)
            {
                yield return parent;
            }
        }

        IEnumerable<string> GetChildDirectories(string path)
        {
            return Directory.GetDirectories(path);
        }

        IEnumerable<string> GetChildFiles(string path)
        {
            return Directory.GetFiles(path);
        }

        TEntity GetEntity<TEntity>(string path)
            where TEntity : IOEntity
        {
            return (TEntity) (File.Exists(path)
                                  ? IOFileEntity.Create(path)
                                  : Directory.Exists(path)
                                        ? (IOEntity) IOCategoryEntity.Create(path)
                                        : IONullEntity.Create(path));
        }
    }
}