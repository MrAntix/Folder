using System;
using System.IO;
using System.Threading;
using Xunit;

namespace Antix.IO.Tests
{
    public class watching_a_file_system_directory
    {
        static IIOSystem GetServiceUnderTest()
        {
            return new IOFileSystem();
        }

        [Fact]
        public void file_created()
        {
            var sut = GetServiceUnderTest();

            var observed = false;

            var tempPath = Path.Combine(Path.GetTempPath(), "create");
            if (!Directory.Exists(tempPath))
                Directory.CreateDirectory(tempPath);

            var tempFile = Path.Combine(tempPath, "file.tmp");
            if (File.Exists(tempFile))
                File.Delete(tempFile);

            Thread.Sleep(2000);

            sut.Watch(
                new IODirectory
                    {
                        Path = tempPath
                    })
                .Subscribe(e =>
                               {
                                   Console.WriteLine(e.Type);
                                   Console.WriteLine(e.Object.Path);

                                   observed = e.Object.Path == tempFile
                                              && e.Type == IOEventType.Created;
                               });

            // act
            File.WriteAllText(
                tempFile,
                "Hello");

            File.WriteAllText(
                tempFile,
                "Hello there");

            Thread.Sleep(10000);

            Assert.True(observed);
        }

        [Fact]
        public void file_updated()
        {
            var sut = GetServiceUnderTest();

            var observed = false;

            var tempPath = Path.Combine(Path.GetTempPath(), "update");
            if (!Directory.Exists(tempPath))
                Directory.CreateDirectory(tempPath);

            var tempFile = Path.Combine(tempPath, "file.tmp");

            File.WriteAllText(
                tempFile,
                "Hello");

            Thread.Sleep(2000);

            sut.Watch(
                new IODirectory
                    {
                        Path = tempPath
                    })
                .Subscribe(e =>
                               {
                                   Console.WriteLine(e.Type);
                                   Console.WriteLine(e.Object.Path);

                                   observed = e.Object.Path == tempFile
                                              && e.Type == IOEventType.Updated;
                               });

            // act
            File.WriteAllText(
                tempFile,
                "Hello there");
            File.Delete(
                tempFile);
            File.WriteAllText(
                tempFile,
                "Hello there");

            Thread.Sleep(3000);

            Assert.True(observed);
        }

        [Fact]
        public void file_deleted()
        {
            var sut = GetServiceUnderTest();

            var observed = false;

            var tempPath = Path.Combine(Path.GetTempPath(), "deleted");
            if (!Directory.Exists(tempPath))
                Directory.CreateDirectory(tempPath);

            var tempFile = Path.Combine(tempPath, "file.tmp");

            File.WriteAllText(
                tempFile,
                "Hello");

            Thread.Sleep(2000);

            sut.Watch(
                new IODirectory
                    {
                        Path = tempPath
                    })
                .Subscribe(e =>
                               {
                                   Console.WriteLine(e.Type);
                                   Console.WriteLine(e.Object.Path);

                                   observed = e.Object.Path == tempFile
                                              && e.Type == IOEventType.Deleted;
                               });

            // act
            File.Delete(
                tempFile);

            Thread.Sleep(3000);

            Assert.True(observed);
        }
    }
}