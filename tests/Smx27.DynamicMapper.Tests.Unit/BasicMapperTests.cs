using System;
using Xunit;
using Smx27.DynamicMapper;
using Smx27.DynamicMapper.Abstractions;

namespace Smx27.DynamicMapper.Tests.Unit
{
    public class BasicMapperTests
    {
        private class TestSource
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
        }

        private class TestDestination
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
        }

        [Fact]
        public void Map_ObjectToObject_ReturnsMappedInstance()
        {
            // Arrange
            var source = new TestSource { Id = 1, Name = "Test" };
            var mapper = new DynamicMapperBuilder().Build();

            // Act
            var result = mapper.Map<TestDestination>(source);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(source.Id, result.Id);
            Assert.Equal(source.Name, result.Name);
        }

        [Fact]
        public void Map_JsonToObject_ReturnsMappedInstance()
        {
            // Arrange
            var json = @"{""Id"": 2, ""Name"": ""JsonTest""}";
            var mapper = new DynamicMapperBuilder().Build();

            // Act
            var result = mapper.Map<TestDestination>(json);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Id);
            Assert.Equal("JsonTest", result.Name);
        }

        [Fact]
        public void MapToDynamic_ReturnsDynamicObject()
        {
            // Arrange
            var source = new TestSource { Id = 3, Name = "DynamicTest" };
            var mapper = new DynamicMapperBuilder().Build();

            // Act
            dynamic result = mapper.MapToDynamic(source);

            // Assert
            Assert.Equal(3, result.Id);
            Assert.Equal("DynamicTest", result.Name);
        }

        [Fact]
        public void Constructor_NullConfiguration_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new DynamicMapper(null!));
        }

        [Fact]
        public void Map_NullSource_ThrowsArgumentNullException()
        {
            // Arrange
            var mapper = new DynamicMapperBuilder().Build();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => mapper.Map<TestDestination>((object)null!));
            Assert.Throws<ArgumentNullException>(() => mapper.Map<TestDestination>((string)null!));
        }
    }
}
