namespace Smx27.DynamicMapper.Abstractions;

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
