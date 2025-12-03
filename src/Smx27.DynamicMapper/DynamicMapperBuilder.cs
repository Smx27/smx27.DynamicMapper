using Newtonsoft.Json;
using Smx27.DynamicMapper.Abstractions;

namespace Smx27.DynamicMapper
{
    /// <summary>
    /// Builder for creating DynamicMapper instances
    /// </summary>
    public class DynamicMapperBuilder
    {
        private MappingConfiguration _configuration = new();
        private JsonSerializerSettings? _jsonSettings;

        /// <summary>
        /// Builds a configured DynamicMapper instance
        /// </summary>
        public IDynamicMapper Build()
        {
            return new DynamicMapper(_configuration);
        }

        /// <summary>
        /// Configures mappings from JSON string
        /// </summary>
        public DynamicMapperBuilder WithJsonConfiguration(string jsonConfig)
        {
            if (string.IsNullOrWhiteSpace(jsonConfig))
                throw new ArgumentNullException(nameof(jsonConfig));

            _configuration = JsonConvert.DeserializeObject<MappingConfiguration>(
                jsonConfig,
                GetJsonSettings()
            ) ?? new MappingConfiguration();

            return this;
        }

        /// <summary>
        /// Configures mappings from MappingConfiguration object
        /// </summary>
        public DynamicMapperBuilder WithConfiguration(MappingConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            return this;
        }

        /// <summary>
        /// Adds a custom JsonSerializerSettings
        /// </summary>
        public DynamicMapperBuilder WithJsonSettings(JsonSerializerSettings settings)
        {
            _jsonSettings = settings;
            return this;
        }

        private JsonSerializerSettings GetJsonSettings()
        {
            return _jsonSettings ?? new JsonSerializerSettings
            {
                ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore
            };
        }
    }
}
