using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Smx27.DynamicMapper.Abstractions;

namespace Smx27.DynamicMapper;

/// <summary>
/// Basic implementation of IDynamicMapper
/// </summary>
public class DynamicMapper : IDynamicMapper
{
        private MappingConfiguration _configuration;
        private readonly JsonSerializerSettings _jsonSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicMapper"/> class with default configuration.
        /// </summary>
        public DynamicMapper() : this(new MappingConfiguration())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicMapper"/> class with specified configuration.
        /// </summary>
        /// <param name="configuration">The mapping configuration.</param>
        public DynamicMapper(MappingConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _jsonSettings = new JsonSerializerSettings
            {
                ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore
            };
        }

        /// <inheritdoc />
        public TDestination Map<TDestination>(object source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            var sourceType = source.GetType();
            var destinationType = typeof(TDestination);

            // For now, use reflection-based mapping
            return MapReflection<TDestination>(source, sourceType, destinationType);
        }

        /// <inheritdoc />
        public TDestination Map<TDestination>(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                throw new ArgumentNullException(nameof(json));

            var jToken = JToken.Parse(json);
            return Map<TDestination>(jToken);
        }

        /// <inheritdoc />
        public TDestination Map<TDestination>(JToken json)
        {
            if (json == null)
                throw new ArgumentNullException(nameof(json));

            // Convert JToken to dictionary for easier processing
            var dict = json.ToObject<Dictionary<string, object>>();
            if (dict == null)
                throw new InvalidOperationException("Failed to parse JSON to dictionary");

            return MapFromDictionary<TDestination>(dict);
        }

        /// <inheritdoc />
        public dynamic MapToDynamic(object source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            var dict = new Dictionary<string, object?>();
            var type = source.GetType();
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var prop in properties)
            {
                dict[prop.Name] = prop.GetValue(source);
            }

            // Convert dictionary to ExpandoObject for dynamic access
            return ConvertToExpando(dict);
        }

        /// <inheritdoc />
        public void ReloadConfiguration(string configJson)
        {
            if (string.IsNullOrWhiteSpace(configJson))
                throw new ArgumentNullException(nameof(configJson));

            _configuration = JsonConvert.DeserializeObject<MappingConfiguration>(
                configJson,
                _jsonSettings
            ) ?? new MappingConfiguration();
        }

        #region Private Methods

        private TDestination MapReflection<TDestination>(
            object source,
            Type sourceType,
            Type destinationType)
        {
            var destination = Activator.CreateInstance<TDestination>();
            if (destination == null)
                 throw new InvalidOperationException($"Could not create instance of {typeof(TDestination).Name}");

            var sourceProperties = sourceType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var destProperties = destinationType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            // Try to find a configured mapping
            var mapping = FindMapping(sourceType, destinationType);

            if (mapping != null && mapping.PropertyMappings.Any())
            {
                ApplyConfiguredMapping(source, destination, mapping);
            }
            else
            {
                // Fallback to convention-based mapping (by property name)
                ApplyConventionMapping(source, destination, sourceProperties, destProperties);
            }

            return destination;
        }

        private TDestination MapFromDictionary<TDestination>(Dictionary<string, object> dict)
        {
            var destination = Activator.CreateInstance<TDestination>();
             if (destination == null)
                 throw new InvalidOperationException($"Could not create instance of {typeof(TDestination).Name}");

            var destProperties = typeof(TDestination).GetProperties(
                BindingFlags.Public | BindingFlags.Instance);

            foreach (var prop in destProperties)
            {
                if (dict.TryGetValue(prop.Name, out var value) ||
                    dict.TryGetValue(prop.Name.ToLowerInvariant(), out value) ||
                    dict.TryGetValue(prop.Name.ToUpperInvariant(), out value))
                {
                    SetPropertyValue(destination, prop, value);
                }
            }

            return destination;
        }

        private void ApplyConfiguredMapping(
            object source,
            object destination,
            TypeMapping mapping)
        {
            var sourceType = source.GetType();

            foreach (var propMapping in mapping.PropertyMappings)
            {
                var sourceProp = sourceType.GetProperty(propMapping.SourceProperty);
                var destProp = destination.GetType().GetProperty(propMapping.DestinationProperty);

                if (sourceProp != null && destProp != null)
                {
                    var value = sourceProp.GetValue(source);

                    // Apply transformation if specified
                    if (!string.IsNullOrEmpty(propMapping.Transformation))
                    {
                        value = ApplyTransformation(value, propMapping.Transformation);
                    }

                    if (value != null)
                    {
                        SetPropertyValue(destination, destProp, value);
                    }
                }
            }
        }

        private void ApplyConventionMapping(
            object source,
            object destination,
            PropertyInfo[] sourceProperties,
            PropertyInfo[] destProperties)
        {
            foreach (var destProp in destProperties)
            {
                var sourceProp = sourceProperties.FirstOrDefault(p =>
                    p.Name.Equals(destProp.Name, StringComparison.OrdinalIgnoreCase));

                if (sourceProp != null)
                {
                    var value = sourceProp.GetValue(source);
                    if (value != null) {
                        SetPropertyValue(destination, destProp, value);
                    }
                }
            }
        }

        private void SetPropertyValue(object destination, PropertyInfo property, object value)
        {
            if (value == null)
                return;

            try
            {
                // Simple type conversion
                var targetType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                var convertedValue = Convert.ChangeType(value, targetType);
                property.SetValue(destination, convertedValue);
            }
            catch (Exception ex)
            {
                // Log or handle conversion errors
                throw new InvalidOperationException(
                    $"Failed to set property {property.Name}: {ex.Message}", ex);
            }
        }

        private object? ApplyTransformation(object? value, string transformation)
        {
            // TODO: Implement transformation logic
            return value;
        }

        private TypeMapping? FindMapping(Type sourceType, Type destinationType)
        {
            var mappingKey = $"{sourceType.Name}_{destinationType.Name}";
            if (_configuration.Mappings.TryGetValue(mappingKey, out var mapping))
                return mapping;

            return null;
        }

        private dynamic ConvertToExpando(Dictionary<string, object?> dict)
        {
            var expando = new ExpandoObject();
            var expandoDict = (IDictionary<string, object?>)expando;

            foreach (var kvp in dict)
            {
                expandoDict[kvp.Key] = kvp.Value;
            }

            return expando;
        }

    #endregion
}
