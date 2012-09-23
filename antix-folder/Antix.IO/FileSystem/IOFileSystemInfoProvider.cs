using System;
using System.Collections.Generic;
using System.IO;
using Antix.IO.Entities;
using Antix.IO.Entities.Base;

namespace Antix.IO.FileSystem
{
    public class IOFileSystemInfoProvider : IIOFileSystemInfoProvider
    {
        IEnumerable<IOCategoryEntity> IIOFileSystemInfoProvider
            .GetParentCategories(IOEntity entity)
        {
            throw new NotImplementedException();
        }

        IEnumerable<IOEntity> IIOFileSystemInfoProvider
            .GetChildEntities(IOCategoryEntity entity)
        {
            throw new NotImplementedException();
        }

        IOEntity IIOFileSystemInfoProvider
            .GetEntity(string path)
        {
            if (string.IsNullOrWhiteSpace(path)) throw new ArgumentException("identifier");

            return File.Exists(path)
                       ? new IOFileEntity {Identifier = path}
                       : Directory.Exists(path)
                             ? (IOEntity) new IOCategoryEntity {Identifier = path}
                             : new IONullEntity {Identifier = path};
        }
    }
}