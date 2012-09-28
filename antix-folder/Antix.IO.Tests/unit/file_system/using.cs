using Antix.IO.Entities;
using Antix.IO.Entities.Base;
using Moq;
using Xunit;

namespace Antix.IO.Tests.unit.file_system
{
    public class using_a_file_system :
        _base
    {
        [Fact]
        public void get_info_calls_provider()
        {
            const string path = "An Identifier";

            var mock = GetInfoProviderMock();
            mock
                .Setup(o => o.GetEntity<IOEntity>(path))
                .Verifiable();

            var sut = GetServiceUnderTest(mock);

            sut.GetEntity<IOEntity>(path);

            mock.VerifyAll();
        }

        [Fact]
        public void watch_calls_watcher()
        {
            var entity = IOFileEntity
                .Create("An Identifier");

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
            var entity = IOFileEntity
                .Create("An Identifier");

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
            var entity = IOFileEntity
                .Create("An Identifier");

            var mock = GetWatcherMock();
            mock
                .Setup(o => o.Watch(entity, It.IsAny<IOWatchSettings>()))
                .Verifiable();

            var sut = GetServiceUnderTest(watcherMock: mock);

            sut.Watch(entity, IOWatchSettings.Default);

            mock.VerifyAll();
        }

        [Fact]
        public void ancestors_calls_back_to_info_provider()
        {
            var entity = IOFileEntity.Create("A Path");

            var infoProviderMock = GetInfoProviderMock();
            infoProviderMock
                .Setup(x => x.GetParentDirectories(entity.Identifier))
                .Returns(new string[] {})
                .Verifiable();

            var sut = GetServiceUnderTest(infoProviderMock: infoProviderMock);

            sut.GetAncestorsAsync(entity);

            infoProviderMock.VerifyAll();
        }


        [Fact]
        public void parents_calls_back_to_info_provider()
        {
            var entity = IOFileEntity.Create("A Path");

            var infoProviderMock = GetInfoProviderMock();
            infoProviderMock
                .Setup(x => x.GetParentDirectory(entity.Identifier))
                .Verifiable();

            var sut = GetServiceUnderTest(infoProviderMock: infoProviderMock);

            sut.GetParentsAsync(entity);

            infoProviderMock.VerifyAll();
        }
    }
}