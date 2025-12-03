# Smx27.DynamicMapper

A dynamic mapping library for .NET with JSON-based configuration.

## Quick Start

### Installation

```bash
dotnet add package Smx27.DynamicMapper
```

### Basic Usage

```csharp
using Smx27.DynamicMapper;

// Create mapper
var mapper = new DynamicMapperBuilder().Build();

// Map from object
var destination = mapper.Map<DestinationModel>(sourceObject);

// Map from JSON
var fromJson = mapper.Map<DestinationModel>("{\"Id\": 1, \"Name\": \"Test\"}");

// Map to dynamic
dynamic dynamicResult = mapper.MapToDynamic(sourceObject);
```

### Configure Mappings

```csharp
var configJson = @"{
    ""Mappings"": {
        ""Source_Destination"": {
            ""SourceType"": ""Namespace.Source, Assembly"",
            ""DestinationType"": ""Namespace.Destination, Assembly"",
            ""PropertyMappings"": [
                { ""SourceProperty"": ""Id"", ""DestinationProperty"": ""Id"" },
                { ""SourceProperty"": ""FullName"", ""DestinationProperty"": ""Name"" }
            ]
        }
    }
}";

var mapper = new DynamicMapperBuilder()
    .WithJsonConfiguration(configJson)
    .Build();
```

## Features

*   ✅ Object-to-object mapping
*   ✅ JSON-to-object mapping
*   ✅ Dynamic object creation
*   ✅ JSON-based configuration
*   ✅ .NET 8+ support
