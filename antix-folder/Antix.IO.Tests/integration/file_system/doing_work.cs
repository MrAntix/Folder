using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Antix.IO.FileSystem;
using Xunit;

namespace Antix.IO.Tests.integration.file_system
{
    public class doing_work
    {
        IIOFileSystemWorker GetServiceUnderTest()
        {
            return new IOFileSystemWorker();
        }

        [Fact]
        public void can_create_a_file()
        {
            var path = Path.GetTempFileName();
            File.Delete(path);

            var sut = GetServiceUnderTest();

            using (var stream = sut.CreateFile(path))
            {
                stream.WriteByte(65);
            }

            Assert.True(File.Exists(path));
        }

        [Fact]
        public void can_create_a_directory()
        {
            var path = Path.GetTempFileName();
            File.Delete(path);

            var sut = GetServiceUnderTest();

            sut.CreateDirectory(path);

            Assert.True(Directory.Exists(path));
        }

        [Fact]
        public void can_create_a_directory_tree()
        {
            var path = Path.GetTempFileName();
            File.Delete(path);

            path = Path.Combine(path, "test", "test");

            var sut = GetServiceUnderTest();

            sut.CreateDirectory(path);

            Assert.True(Directory.Exists(path));
        }

        [Fact]
        public void can_delete_a_file()
        {
            var path = Path.GetTempFileName();

            var sut = GetServiceUnderTest();

            sut.DeleteFile(path);

            Assert.False(File.Exists(path));
        }

        [Fact]
        public void can_delete_a_directory()
        {
            var path = Path.GetTempFileName();
            File.Delete(path);
            Directory.CreateDirectory(path);

            var sut = GetServiceUnderTest();

            sut.DeleteDirectory(path);

            Assert.False(Directory.Exists(path));
        }

        [Fact]
        public void cannot_delete_a_non_empty_directory_without_setting()
        {
            var path = Path.GetTempFileName();
            File.Delete(path);
            Directory.CreateDirectory(Path.Combine(path, "test", "test"));

            var sut = GetServiceUnderTest();

            Assert.Throws<IOException>(() =>
                                       sut.DeleteDirectory(path));
        }

        [Fact]
        public void can_delete_a_non_empty_directory_with_setting()
        {
            var path = Path.GetTempFileName();
            File.Delete(path);
            Directory.CreateDirectory(Path.Combine(path, "test", "test"));

            var sut = GetServiceUnderTest();

            sut.DeleteDirectoryRecursive(path);

            Assert.False(Directory.Exists(path));
        }

        [Fact]
        public async Task can_open_a_file_for_read()
        {
            const string text = "hello";

            var path = Path.GetTempFileName();
            File.WriteAllText(path, text);

            var sut = GetServiceUnderTest();

            using (var stream = await sut.OpenReadAsync(path))
            using (var reader = new StreamReader(stream))
            {
                Assert.Equal(text, reader.ReadToEnd());
            }
        }

        [Fact]
        public async Task can_open_a_file_for_write()
        {
            const string text = "hello";

            var path = Path.GetTempFileName();

            var sut = GetServiceUnderTest();

            using (var stream = await sut.OpenWriteAsync(path))
            using (var writer = new StreamWriter(stream))
            {
                writer.Write(text);
            }

            Assert.Equal(text, File.ReadAllText(path));
        }

        [Fact]
        public async Task waits_for_locked_file()
        {
            const string text = "hello";

            var path = Path.GetTempFileName();

            var sut = GetServiceUnderTest();

            Task.Run(() =>
                         {

                             using (var lockingStream = File.OpenWrite(path))
                             using (var writer = new StreamWriter(lockingStream))
                             {
                                 writer.Write(text);
                                 Thread.Sleep(500);
                             }
                         });

            using (var stream = await sut.OpenReadAsync(path))
            using (var reader = new StreamReader(stream))
            {
                Assert.Equal(text, reader.ReadToEnd());
            }
        }
    }
}