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
            if (string.IsNullOrWhiteSpace(path)) throw new ArgumentException("path");

            return File.Exists(path)
                       ? new IOFileEntity {Path = path}
                       : Directory.Exists(path)
                             ? (IOEntity) new IOCategoryEntity {Path = path}
                             : new IONullEntity {Path = path};
        }
    }
}