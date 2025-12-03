using System.Collections.Generic;

namespace Smx27.DynamicMapper.Abstractions
{
    /// <summary>
    /// Represents a single property mapping configuration
    /// </summary>
    public class PropertyMapping
    {
        /// <summary>
        /// Gets or sets the name of the property on the source object.
        /// </summary>
        public string SourceProperty { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the name of the property on the destination object.
        /// </summary>
        public string DestinationProperty { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the transformation expression to apply.
        /// </summary>
        public string? Transformation { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this mapping is required.
        /// </summary>
        public bool IsRequired { get; set; } = false;

        /// <summary>
        /// Gets or sets the default value to use if source is null/missing.
        /// </summary>
        public object? DefaultValue { get; set; }
    }

    /// <summary>
    /// Represents a complete mapping configuration between two types
    /// </summary>
    public class TypeMapping
    {
        /// <summary>
        /// Gets or sets the unique name of this mapping configuration.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the fully qualified name of the source type.
        /// </summary>
        public string SourceType { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the fully qualified name of the destination type.
        /// </summary>
        public string DestinationType { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the list of property mappings.
        /// </summary>
        public List<PropertyMapping> PropertyMappings { get; set; } = new();

        /// <summary>
        /// Gets or sets the settings for handling dynamic properties.
        /// </summary>
        public DynamicPropertySettings DynamicSettings { get; set; } = new();
    }

    /// <summary>
    /// Root configuration containing all mappings
    /// </summary>
    public class MappingConfiguration
    {
        /// <summary>
        /// Gets or sets the collection of type mappings keyed by name.
        /// </summary>
        public Dictionary<string, TypeMapping> Mappings { get; set; } = new();

        /// <summary>
        /// Gets or sets the configuration version.
        /// </summary>
        public string Version { get; set; } = "1.0";
    }
}
