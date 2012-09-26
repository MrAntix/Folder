using Antix.IO.FileSystem;
using Moq;

namespace Antix.IO.Tests.unit.file_system
{
    public abstract class _base
    {
        protected IIOSystem GetServiceUnderTest(
            Mock<IIOFileSystemInfoProvider> infoProviderMock = null,
            Mock<IIOFileSystemWatcher> watcherMock = null,
            Mock<IIOFileSystemWorker> workerMock = null)
        {
            infoProviderMock = infoProviderMock ?? GetInfoProviderMock();
            watcherMock = watcherMock ?? GetWatcherMock();
            workerMock = workerMock ?? GetWorker();

            return new IOFileSystem(
                watcherMock.Object,
                infoProviderMock.Object,
                workerMock.Object
                );
        }

        protected Mock<IIOFileSystemWatcher> GetWatcherMock()
        {
            var mock = new Mock<IIOFileSystemWatcher>();

            return mock;
        }

        protected Mock<IIOFileSystemInfoProvider> GetInfoProviderMock()
        {
            var mock = new Mock<IIOFileSystemInfoProvider>();

            return mock;
        }

        protected Mock<IIOFileSystemWorker> GetWorker()
        {
            var mock = new Mock<IIOFileSystemWorker>();

            return mock;
        }
    }
}