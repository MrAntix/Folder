using System;
using System.Collections.Generic;
using System.IO;
using Antix.IO.Entities;
using Antix.IO.Entities.Base;

namespace Antix.IO.FileSystem
{
    public class IOFileSystemInfoProvider : IIOFileSystemInfoProvider
    {
        IOCategoryEntity IIOFileSystemInfoProvider
            .GetParentDirectory(IOEntity entity)
        {
            return new IOCategoryEntity
                       {
                           Identifier = GetParentDirectory(entity.Identifier)
                       };
        }

        IEnumerable<IOEntity> IIOFileSystemInfoProvider
            .GetChildDirectoriesAndFiles(IOCategoryEntity entity)
        {
            foreach (var path in GetChildDirectories(entity.Identifier))
            {
                yield return new IOCategoryEntity {Identifier = path};
            }
            foreach (var path in GetChildFiles(entity.Identifier))
            {
                yield return new IOFileEntity {Identifier = path};
            }
        }

        IOEntity IIOFileSystemInfoProvider
            .GetEntity(string path)
        {
            if (string.IsNullOrWhiteSpace(path)) throw new ArgumentException("identifier");

            return GetEntity(path);
        }

        string GetParentDirectory(string path)
        {
            return
                Path.GetDirectoryName(path);
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
                       ? new IOFileEntity {Identifier = path}
                       : Directory.Exists(path)
                             ? (IOEntity) new IOCategoryEntity {Identifier = path}
                             : new IONullEntity {Identifier = path};
        }
    }
}