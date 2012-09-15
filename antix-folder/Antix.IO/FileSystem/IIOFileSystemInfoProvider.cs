using Antix.IO.Entities.Base;

namespace Antix.IO.FileSystem
{
    public interface IIOFileSystemInfoProvider
    {
        IOEntity GetInfo(string path);
    }
}