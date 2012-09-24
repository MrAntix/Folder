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

        IOEntity IIOFileSystemInfoProvider
            .GetEntity(string path)
        {
            if (string.IsNullOrWhiteSpace(path)) throw new ArgumentException("identifier");

            return GetEntity(path);
        }

        IOFileEntity IIOFileSystemInfoProvider.GetFileEntity(string path)
        {
            return GetFileEntity(path);
        }

        IOCategoryEntity IIOFileSystemInfoProvider.GetCategoryEntity(string path)
        {
            return GetCategoryEntity(path);
        }

        IONullEntity IIOFileSystemInfoProvider.GetNullEntity(string path)
        {
            return GetNullEntity(path);
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


        IOEntity GetEntity(string path)
        {
            return File.Exists(path)
                       ? GetFileEntity(path)
                       : Directory.Exists(path)
                             ? (IOEntity) GetCategoryEntity(path)
                             : GetNullEntity(path);
        }

        IOFileEntity GetFileEntity(string path)
        {
            return IOFileEntity.Create(path);
        }

        IOCategoryEntity GetCategoryEntity(string path)
        {
            return IOCategoryEntity.Create(path);
        }

        IONullEntity GetNullEntity(string path)
        {
            return IONullEntity.Create(path);
        }
    }
}