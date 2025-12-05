using System;
using System.Collections.Generic;
using Xunit;
using Smx27.DynamicMapper;
using Smx27.DynamicMapper.Abstractions;
using Smx27.DynamicMapper.Abstractions.Exceptions;

namespace Smx27.DynamicMapper.Tests.Unit
{
    public class DynamicBehaviorTests
    {
        private class SourceWithExtras
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public string ExtraField { get; set; } = string.Empty;
            public decimal ExtraNumber { get; set; }
        }

        private class DestinationWithDictionary
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public Dictionary<string, object> Metadata { get; set; } = new();
        }

        private class DestinationWithoutDictionary
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
        }

        [Fact]
        public void StoreInDictionaryBehavior_StoresUnmappedProperties()
        {
            // Arrange
            var config = @"{
                ""Mappings"": {
                    ""TestMapping"": {
                        ""SourceType"": ""Smx27.DynamicMapper.Tests.Unit.DynamicBehaviorTests+SourceWithExtras, Smx27.DynamicMapper.Tests.Unit"",
                        ""DestinationType"": ""Smx27.DynamicMapper.Tests.Unit.DynamicBehaviorTests+DestinationWithDictionary, Smx27.DynamicMapper.Tests.Unit"",
                        ""PropertyMappings"": [
                            { ""SourceProperty"": ""Id"", ""DestinationProperty"": ""Id"" },
                            { ""SourceProperty"": ""Name"", ""DestinationProperty"": ""Name"" }
                        ],
                        ""DynamicSettings"": {
                            ""Behavior"": ""StoreInDictionary"",
                            ""DictionaryPropertyName"": ""Metadata""
                        }
                    }
                }
            }";

            var mapper = new DynamicMapperBuilder()
                .WithJsonConfiguration(config)
                .Build();

            var source = new SourceWithExtras
            {
                Id = 1,
                Name = "Test",
                ExtraField = "Extra Value",
                ExtraNumber = 99.99m
            };

            // Act
            var result = mapper.Map<DestinationWithDictionary>(source);

            // Assert
            Assert.NotNull(result.Metadata);
            Assert.Equal(2, result.Metadata.Count);
            Assert.Equal("Extra Value", result.Metadata["ExtraField"]);
            Assert.Equal(99.99m, result.Metadata["ExtraNumber"]);
        }

        [Fact]
        public void ThrowExceptionBehavior_ThrowsOnUnmappedProperties()
        {
            // Arrange
            var config = @"{
                ""Mappings"": {
                    ""TestMapping"": {
                        ""SourceType"": ""Smx27.DynamicMapper.Tests.Unit.DynamicBehaviorTests+SourceWithExtras, Smx27.DynamicMapper.Tests.Unit"",
                        ""DestinationType"": ""Smx27.DynamicMapper.Tests.Unit.DynamicBehaviorTests+DestinationWithoutDictionary, Smx27.DynamicMapper.Tests.Unit"",
                        ""PropertyMappings"": [
                            { ""SourceProperty"": ""Id"", ""DestinationProperty"": ""Id"" }
                        ],
                        ""DynamicSettings"": {
                            ""Behavior"": ""ThrowException""
                        }
                    }
                }
            }";

            var mapper = new DynamicMapperBuilder()
                .WithJsonConfiguration(config)
                .Build();

            var source = new SourceWithExtras
            {
                Id = 1,
                Name = "Test",
                ExtraField = "Extra"
            };

            // Act & Assert
            var exception = Assert.Throws<UnmappedPropertyException>(() =>
                mapper.Map<DestinationWithoutDictionary>(source));

            Assert.Contains("Unmapped properties", exception.Message);
        }

        [Fact]
        public void IgnoreBehavior_DoesNotStoreUnmappedProperties()
        {
            // Arrange
            var config = @"{
                ""Mappings"": {
                    ""TestMapping"": {
                        ""SourceType"": ""Smx27.DynamicMapper.Tests.Unit.DynamicBehaviorTests+SourceWithExtras, Smx27.DynamicMapper.Tests.Unit"",
                        ""DestinationType"": ""Smx27.DynamicMapper.Tests.Unit.DynamicBehaviorTests+DestinationWithDictionary, Smx27.DynamicMapper.Tests.Unit"",
                        ""PropertyMappings"": [
                            { ""SourceProperty"": ""Id"", ""DestinationProperty"": ""Id"" }
                        ],
                        ""DynamicSettings"": {
                            ""Behavior"": ""Ignore""
                        }
                    }
                }
            }";

            var mapper = new DynamicMapperBuilder()
                .WithJsonConfiguration(config)
                .Build();

            var source = new SourceWithExtras
            {
                Id = 1,
                Name = "Test",
                ExtraField = "Extra"
            };

            // Act
            var result = mapper.Map<DestinationWithDictionary>(source);

            // Assert
            Assert.NotNull(result.Metadata);
            Assert.Empty(result.Metadata);
        }
    }
}
