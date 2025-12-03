using Newtonsoft.Json.Linq;

namespace Smx27.DynamicMapper.Abstractions
{
    /// <summary>
    /// Core mapping interface for dynamic object mapping with JSON configuration
    /// </summary>
    public interface IDynamicMapper
    {
        /// <summary>
        /// Maps an object to a specified type
        /// </summary>
        TDestination Map<TDestination>(object source);

        /// <summary>
        /// Maps JSON string to a specified type
        /// </summary>
        TDestination Map<TDestination>(string json);

        /// <summary>
        /// Maps JToken to a specified type
        /// </summary>
        TDestination Map<TDestination>(JToken json);

        /// <summary>
        /// Maps to a dynamic/expando object
        /// </summary>
        dynamic MapToDynamic(object source);

        /// <summary>
        /// Reloads mapping configuration from JSON
        /// </summary>
        void ReloadConfiguration(string configJson);
    }
}
