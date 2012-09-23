using System;
using System.IO;
using Antix.IO.Entities;
using Antix.IO.Entities.Base;

namespace Antix.IO.FileSystem
{
    public class IOFileSystemInfoProvider : IIOFileSystemInfoProvider
    {
        public IOEntity GetInfo(string path)
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