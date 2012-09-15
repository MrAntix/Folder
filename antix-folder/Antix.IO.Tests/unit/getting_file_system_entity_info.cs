using System.IO;
using Antix.IO.Entities;
using Antix.IO.FileSystem;
using Xunit;

namespace Antix.IO.Tests.unit
{
    public class getting_file_system_entity_info
    {
        static IIOFileSystemInfoProvider GetServiceUnderTest()
        {
            return new IOFileSystemInfoProvider();
        }

        [Fact]
        public void gets_info_on_a_directory()
        {
            var path = Path.GetTempPath();

            var sut = GetServiceUnderTest();

            var result = sut.GetInfo(path);

            Assert.IsType<IODirectoryEntity>(result);
        }

        [Fact]
        public void gets_info_on_a_file()
        {
            var path = Path.GetTempFileName();

            var sut = GetServiceUnderTest();

            var result = sut.GetInfo(path);

            Assert.IsType<IOFileEntity>(result);
        }

        [Fact]
        public void gets_null_entity_where_path_does_not_exist()
        {
            var path = Path.GetTempFileName();
            File.Delete(path);

            var sut = GetServiceUnderTest();

            var result = sut.GetInfo(path);

            Assert.IsType<IONullEntity>(result);
        }
    }
}