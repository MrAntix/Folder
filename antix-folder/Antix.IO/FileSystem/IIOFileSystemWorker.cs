using System.IO;
using System.Threading.Tasks;

namespace Antix.IO.FileSystem
{
    /// <summary>
    ///   <para> Does work on a filesystem, create, copy and delete </para>
    /// </summary>
    public interface IIOFileSystemWorker
    {
        /// <summary>
        ///   <para> Create a file pass back the stream </para>
        /// </summary>
        Stream CreateFile(string path);

        /// <summary>
        ///   <para> Create a directory </para>
        /// </summary>
        void CreateDirectory(string path);

        /// <summary>
        ///   <para> Delete a file </para>
        /// </summary>
        void DeleteFile(string path);

        /// <summary>
        ///   <para> Delete a directory </para>
        /// </summary>
        void DeleteDirectory(string path);

        /// <summary>
        ///   <para> Delete a directory </para>
        /// </summary>
        void DeleteDirectoryRecursive(string path);

        /// <summary>
        ///   <para> Open a file for reading </para>
        ///   <para> If file is locked will wait </para>
        /// </summary>
        Task<Stream> OpenReadAsync(string path);

        /// <summary>
        ///   <para> Open a file for writing </para>
        ///   <para> If file is locked will wait </para>
        /// </summary>
        Task<Stream> OpenWriteAsync(string path);
    }
}