using System.IO;
using Antix.IO.Entities;
using Antix.IO.FileSystem;
using Xunit;

namespace Antix.IO.Tests.integration.file_system
{
    public class getting_entities
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
            var entity = sut.GetEntity(Path.GetTempFileName());

            var result = sut.GetParentDirectory(entity);

            Assert.IsType<IOCategoryEntity>(result);
        }

        [Fact]
        public void gets_child_files_and_directories()
        {
            var sut = GetServiceUnderTest();
            var entity = (IOCategoryEntity)sut.GetEntity(Path.GetTempPath());

            var result = sut.GetChildDirectoriesAndFiles(entity);

            Assert.NotNull(result);
        }
    }
}