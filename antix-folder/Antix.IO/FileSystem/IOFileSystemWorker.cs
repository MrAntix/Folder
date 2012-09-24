using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Antix.IO.FileSystem
{
    public class IOFileSystemWorker : IIOFileSystemWorker
    {
        public Stream CreateFile(string path)
        {
            return File.Create(path);
        }

        public void CreateDirectory(string path)
        {
            Directory.CreateDirectory(path);
        }

        public void DeleteFile(string path)
        {
            File.Delete(path);
        }

        public void DeleteDirectory(string path)
        {
            Directory.Delete(path);
        }

        public void DeleteDirectoryRecursive(string path)
        {
            Directory.Delete(path, true);
        }

        public async Task<Stream> OpenReadAsync(string path)
        {
            return await Task.FromResult(
                ShareWait(() => File.OpenRead(path)));
        }

        public async Task<Stream> OpenWriteAsync(string path)
        {
            return await Task.FromResult(
                ShareWait(() => File.OpenWrite(path)));
        }

        static Stream ShareWait(Func<Stream> open)
        {
            var retry = 0;
            while (true)
            {
                try
                {
                    return open();
                }
                catch (IOException ex)
                {
                    if (retry == 10) throw;
                    if (!IsSharingViolation(ex)) throw;

                    retry++;

                    var wait = (int) Math.Pow(2, retry + 7); // ~2mins max
                    Debug.WriteLine("Waiting on lock {0}", wait);
                    Thread.Sleep(wait);
                }
            }
        }

        const int HResultSharingViolation = -2147024864;

        static bool IsSharingViolation(
            IOException ioEx)
        {
            var field = ioEx
                .GetType().GetField("_HResult", BindingFlags.NonPublic | BindingFlags.Instance);
            if (field == null) return false;

            var hres = (int) field.GetValue(ioEx);
            Debug.WriteLine("Error '{0}' {1}", ioEx.Message, hres);

            return hres == HResultSharingViolation;
        }
    }
}