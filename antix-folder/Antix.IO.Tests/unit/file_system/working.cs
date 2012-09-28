using System.IO;
using Antix.IO.Entities;
using Xunit;

namespace Antix.IO.Tests.unit.file_system
{
    public class working :
        _base
    {
        [Fact]
        public void can_create_a_file()
        {
            const string path = "A Path";

            var workerMock = GetWorker();
            workerMock
                .Setup(x => x.CreateFile(path))
                .Returns(new MemoryStream())
                .Verifiable();

            var sut = GetServiceUnderTest(workerMock: workerMock);

            using (sut.CreateFile(path))
            {
            }

            workerMock.Verify();
        }

        [Fact]
        public void can_delete_a_file()
        {
            const string path = "A Path";

            var workerMock = GetWorker();
            workerMock
                .Setup(x => x.DeleteFile(path))
                .Verifiable();

            var infoProviderMock = GetInfoProviderMock();
            infoProviderMock
                .Setup(x => x.GetEntity<IOFileEntity>(path))
                .Returns(IOFileEntity.Create(path));

            var sut = GetServiceUnderTest(
                workerMock: workerMock, 
                infoProviderMock: infoProviderMock);

            sut.DeleteFile(path);

            workerMock.Verify();
        }
    }
}