using System.Collections.Generic;

namespace Smx27.DynamicMapper.Abstractions;

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
