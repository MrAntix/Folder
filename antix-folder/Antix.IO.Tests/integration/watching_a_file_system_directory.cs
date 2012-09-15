﻿using System;
using System.IO;
using System.Threading;
using Antix.IO.Entities;
using Antix.IO.Events;
using Antix.IO.FileSystem;
using Xunit;

namespace Antix.IO.Tests.integration
{
    public class watching_a_file_system_directory
    {
        static IOFileSystemWatcher GetServiceUnderTest()
        {
            var infoProvider = new IOFileSystemInfoProvider();

            return new IOFileSystemWatcher(infoProvider);
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

            Thread.Sleep(100);

            sut.Watch(
                new IODirectoryEntity
                    {
                        Path = tempPath
                    },
                new IOWatchSettings()
                )
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

            Thread.Sleep(2100);

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

            Thread.Sleep(100);

            sut.Watch(
                new IODirectoryEntity
                    {
                        Path = tempPath
                    },
                new IOWatchSettings()
                )
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

            Thread.Sleep(2100);

            Assert.True(observed);
        }

        [Fact]
        public void file_moved()
        {
            var sut = GetServiceUnderTest();

            var observed = false;

            var tempDirectory = Path.Combine(Path.GetTempPath(), "rename");
            if (!Directory.Exists(tempDirectory))
                Directory.CreateDirectory(tempDirectory);

            var tempFile = Path.Combine(tempDirectory, "file.tmp");
            var tempFileMoved = Path.Combine(tempDirectory, "fileMoved.tmp");
            if (File.Exists(tempFileMoved)) File.Delete(tempFileMoved);

            File.WriteAllText(
                tempFile,
                "Hello");

            Thread.Sleep(100);

            sut.Watch(
                new IODirectoryEntity
                    {
                        Path = tempDirectory
                    },
                new IOWatchSettings()
                )
                .Subscribe(e =>
                               {
                                   Console.WriteLine(e);

                                   var movedEvent = e as IOMovedEvent;
                                   if (movedEvent != null
                                       && movedEvent.Entity.Path == tempFile
                                       && movedEvent.NewEntity.Path == tempFileMoved)
                                       observed = true;
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

            Thread.Sleep(2100);

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

            Thread.Sleep(100);

            sut.Watch(
                new IODirectoryEntity
                    {
                        Path = tempPath
                    },
                new IOWatchSettings()
                )
                .Subscribe(e =>
                               {
                                   Console.WriteLine(e);

                                   observed = e.Entity.Path == tempFile
                                              && e is IODeletedEvent;
                               });

            // act
            File.Delete(
                tempFile);

            Thread.Sleep(2100);

            Assert.True(observed);
        }

        [Fact]
        public void error_raised_on_directory_deleted()
        {
            var observed = false;
            var tempDirectory = Path.Combine(Path.GetTempPath(), "todelete");
            if (!Directory.Exists(tempDirectory))
                Directory.CreateDirectory(tempDirectory);

            var sut = GetServiceUnderTest();
            sut.Watch(
                new IODirectoryEntity
                    {
                        Path = tempDirectory
                    },
                new IOWatchSettings()
                )
                .Subscribe(
                    Console.WriteLine,
                    ex => { observed = true; });

            // act
            Directory.Delete(tempDirectory);

            Thread.Sleep(100);

            Assert.True(observed);
        }
    }
}