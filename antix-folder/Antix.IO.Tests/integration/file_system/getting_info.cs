using System.Diagnostics;
using System.IO;
using System.Linq;
using Antix.IO.Entities;
using Antix.IO.FileSystem;
using Xunit;

namespace Antix.IO.Tests.integration.file_system
{
    public class getting_info
    {
        static IIOFileSystemInfoProvider GetServiceUnderTest()
        {
            return new IOFileSystemInfoProvider();
        }

        [Fact]
        public void gets_a_directory_entity()
        {
            var path = Path.GetTempPath();

            var sut = GetServiceUnderTest();

            var result = sut.GetEntity(path);

            Assert.IsType<IOCategoryEntity>(result);
        }

        [Fact]
        public void gets_a_file_entity()
        {
            var path = Path.GetTempFileName();

            var sut = GetServiceUnderTest();

            var result = sut.GetEntity(path);

            Assert.IsType<IOFileEntity>(result);
        }

        [Fact]
        public void gets_null_entity_where_path_does_not_exist()
        {
            var path = Path.GetTempFileName();
            File.Delete(path);

            var sut = GetServiceUnderTest();

            var result = sut.GetEntity(path);

            Assert.IsType<IONullEntity>(result);
        }

        [Fact]
        public void gets_parent_directory()
        {
            var sut = GetServiceUnderTest();
            var path = Path.GetTempFileName();

            var result = sut.GetParentDirectory(path);

            Assert.Equal(Path.GetDirectoryName(path), result);
        }

        [Fact]
        public void gets_parent_directories()
        {
            var sut = GetServiceUnderTest();
            var path = Path.GetTempFileName();

            var result = sut.GetParentDirectories(path);

            foreach (var directoryPath in result)
                Debug.WriteLine(directoryPath);

            Assert.Equal(path.Split('\\').Count() - 1, result.Count());
        }

        [Fact]
        public void gets_child_files_and_directories()
        {
            var sut = GetServiceUnderTest();
            var path = Path.GetTempPath();

            var result = sut.GetChildDirectoriesAndFiles(path);

            Assert.NotNull(result);
        }
    }
}