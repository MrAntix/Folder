using System;
using System.IO;
using System.Threading;
using Antix.IO.Entities;
using Antix.IO.Events;
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
                new IODirectoryEntity
                    {
                        Path = tempPath
                    })
                .Subscribe(e =>
                               {
                                   Console.WriteLine(e);

                                   observed = e.Entity.Path == tempFile
                                              && e is IOCreatedEvent;
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
                new IODirectoryEntity
                    {
                        Path = tempPath
                    })
                .Subscribe(e =>
                               {
                                   Console.WriteLine(e);

                                   observed = e.Entity.Path == tempFile
                                              && e is IOUpdatedEvent;
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
        public void file_moved()
        {
            var sut = GetServiceUnderTest();

            var observed = false;

            var tempPath = Path.Combine(Path.GetTempPath(), "rename");
            if (!Directory.Exists(tempPath))
                Directory.CreateDirectory(tempPath);

            var tempFile = Path.Combine(tempPath, "file.tmp");
            var tempFileMoved = Path.Combine(tempPath, "fileMoved.tmp");
            if (File.Exists(tempFileMoved)) File.Delete(tempFileMoved);

            File.WriteAllText(
                tempFile,
                "Hello");

            Thread.Sleep(2000);

            sut.Watch(
                new IODirectoryEntity
                    {
                        Path = tempPath
                    })
                .Subscribe(e =>
                               {
                                   Console.WriteLine(e);

                                   if (e.Entity.Path == tempFile)
                                        observed = e is IOMovedEvent;
                               });

            // act
            File.WriteAllText(
                tempFile,
                "Hello There");
            File.Move(
                tempFile, tempFileMoved);
            File.WriteAllText(
                tempFileMoved,
                "Hello There");

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
                new IODirectoryEntity
                    {
                        Path = tempPath
                    })
                .Subscribe(e =>
                               {
                                   Console.WriteLine(e);

                                   observed = e.Entity.Path == tempFile
                                              && e is IODeletedEvent;
                               });

            // act
            File.Delete(
                tempFile);

            Thread.Sleep(3000);

            Assert.True(observed);
        }
    }
}