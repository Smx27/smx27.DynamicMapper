namespace Smx27.DynamicMapper.Abstractions
{
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

    /// <summary>
    /// Configuration for dynamic property handling
    /// </summary>
    public class DynamicPropertySettings
    {
        /// <summary>
        /// Gets or sets the behavior for handling dynamic properties.
        /// </summary>
        public DynamicMappingBehavior Behavior { get; set; } = DynamicMappingBehavior.Ignore;

        /// <summary>
        /// Gets or sets the name of the dictionary property to store unmapped values.
        /// </summary>
        public string DictionaryPropertyName { get; set; } = "ExtendedProperties";

        /// <summary>
        /// Gets or sets the name of the JObject property to store unmapped values.
        /// </summary>
        public string JObjectPropertyName { get; set; } = "Metadata";
    }
}
