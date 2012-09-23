using Antix.IO.Entities;
using Antix.IO.FileSystem;
using Moq;
using Xunit;

namespace Antix.IO.Tests.unit
{
    public class using_a_file_system
    {
        IIOSystem GetServiceUnderTest(
            Mock<IIOFileSystemInfoProvider> infoProviderMock = null,
            Mock<IIOFileSystemWatcher> watcherMock = null)
        {
            infoProviderMock = infoProviderMock ?? GetInfoProviderMock();
            watcherMock = watcherMock ?? GetWatcherMock();

            return new IOFileSystem(
                watcherMock.Object,
                infoProviderMock.Object
                );
        }

        Mock<IIOFileSystemWatcher> GetWatcherMock()
        {
            var mock = new Mock<IIOFileSystemWatcher>();

            return mock;
        }

        Mock<IIOFileSystemInfoProvider> GetInfoProviderMock()
        {
            var mock = new Mock<IIOFileSystemInfoProvider>();

            return mock;
        }

        [Fact]
        public void get_info_calls_provider()
        {
            const string path = "An Identifier";
            var mock = GetInfoProviderMock();
            mock
                .Setup(o => o.GetEntity(path))
                .Verifiable();

            var sut = GetServiceUnderTest(mock);

            sut.GetEntity(path);

            mock.VerifyAll();
        }

        [Fact]
        public void watch_calls_watcher()
        {
            var entity = new IOFileEntity
                             {
                                 Identifier = "An Identifier"
                             };
            var mock = GetWatcherMock();
            mock
                .Setup(o => o.Watch(entity, It.IsAny<IOWatchSettings>()))
                .Verifiable();

            var sut = GetServiceUnderTest(watcherMock: mock);

            sut.Watch(entity);

            mock.VerifyAll();
        }

        [Fact]
        public void watch_without_settings_passes_default()
        {
            var entity = new IOFileEntity
            {
                Identifier = "An Identifier"
            };
            var mock = GetWatcherMock();
            mock
                .Setup(o => o.Watch(entity, IOWatchSettings.Default))
                .Verifiable();

            var sut = GetServiceUnderTest(watcherMock: mock);

            sut.Watch(entity);

            mock.VerifyAll();
        }

        [Fact]
        public void watch_with_settings_calls_watcher()
        {
            var entity = new IOFileEntity
                             {
                                 Identifier = "An Identifier"
                             };
            var mock = GetWatcherMock();
            mock
                .Setup(o => o.Watch(entity, It.IsAny<IOWatchSettings>()))
                .Verifiable();

            var sut = GetServiceUnderTest(watcherMock: mock);

            sut.Watch(entity, IOWatchSettings.Default);

            mock.VerifyAll();
        }
    }
}