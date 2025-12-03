namespace Smx27.DynamicMapper.Abstractions;

/// <summary>
/// Defines behavior for handling dynamic/unmapped properties
/// </summary>
public enum DynamicMappingBehavior
{
    /// <summary>
    /// Ignore unmapped properties (default)
    /// </summary>
    Ignore,

    /// <summary>
    /// Store unmapped properties in a dictionary property
    /// </summary>
    StoreInDictionary,

    /// <summary>
    /// Throw exception when unmapped properties are found
    /// </summary>
    ThrowException,

    /// <summary>
    /// Store unmapped properties in a JObject property
    /// </summary>
    StoreInJObject
}
