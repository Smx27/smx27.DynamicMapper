using System.Collections.Generic;

namespace Smx27.DynamicMapper.Abstractions;

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
