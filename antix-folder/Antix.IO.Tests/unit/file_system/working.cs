using System.IO;
using Xunit;

namespace Antix.IO.Tests.unit.file_system
{
    public class working :
        _base
    {
        [Fact]
        public void can_create_a_file()
        {
            const string text = "Some text";
            const string path = "A Path";

            var workerMock = GetWorker();
            workerMock
                .Setup(x => x.CreateFile(path))
                .Returns(new MemoryStream())
                .Verifiable();

            var sut = GetServiceUnderTest(workerMock: workerMock);

            using (var file = sut.CreateFile(path))
            {
                file.Write(text);
            }

            workerMock.Verify();
        }
    }
}