using System;
using Newtonsoft.Json;
using Sample.ConsoleApp.Models;
using Smx27.DynamicMapper;
using Smx27.DynamicMapper.Abstractions;

namespace Sample.ConsoleApp;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Smx27.DynamicMapper - Basic Test");
        Console.WriteLine("================================");

        // 1. Create source object
        var source = new SourceModel
        {
            Id = 1,
            Name = "John Doe",
            EmailAddress = "john@example.com",
            CreatedAt = DateTime.Now,
            Amount = 99.99m
        };

        Console.WriteLine("Source Object:");
        Console.WriteLine(JsonConvert.SerializeObject(source, Formatting.Indented));

        // 2. Create mapper with configuration
        var configJson = @"{
            ""Mappings"": {
                ""SourceModel_DestinationModel"": {
                    ""Name"": ""SourceModel_DestinationModel"",
                    ""SourceType"": ""Sample.ConsoleApp.Models.SourceModel, Sample.ConsoleApp"",
                    ""DestinationType"": ""Sample.ConsoleApp.Models.DestinationModel, Sample.ConsoleApp"",
                    ""PropertyMappings"": [
                        { ""SourceProperty"": ""Id"", ""DestinationProperty"": ""Id"" },
                        { ""SourceProperty"": ""Name"", ""DestinationProperty"": ""FullName"" },
                        { ""SourceProperty"": ""EmailAddress"", ""DestinationProperty"": ""Email"" },
                        {
                            ""SourceProperty"": ""CreatedAt"",
                            ""DestinationProperty"": ""CreationDate"",
                            ""Transformation"": ""ToString('yyyy-MM-dd')""
                        },
                        {
                            ""SourceProperty"": ""Amount"",
                            ""DestinationProperty"": ""TotalAmount""
                        }
                    ],
                    ""DynamicSettings"": {
                        ""Behavior"": ""Ignore"",
                        ""DictionaryPropertyName"": ""ExtendedProperties""
                    }
                }
            },
            ""Version"": ""1.0""
        }";

        // 3. Create mapper
        var mapper = new DynamicMapperBuilder()
            .WithJsonConfiguration(configJson)
            .Build();

        // 4. Perform mapping
        var destination = mapper.Map<DestinationModel>(source);

        Console.WriteLine("\nMapped Destination Object:");
        Console.WriteLine(JsonConvert.SerializeObject(destination, Formatting.Indented));

        // 5. Test JSON mapping
        // Note: Since Map<T>(json) performs direct property matching, we first deserialize to SourceModel
        // to utilize the configured SourceModel -> DestinationModel mapping.
        var jsonSource = @"{
            ""Id"": 2,
            ""Name"": ""Jane Smith"",
            ""EmailAddress"": ""jane@example.com"",
            ""CreatedAt"": ""2024-01-01T10:00:00Z"",
            ""Amount"": 149.99
        }";

        // Map JSON to SourceModel (direct property match)
        var sourceFromJson = mapper.Map<SourceModel>(jsonSource);
        // Map SourceModel to DestinationModel (using configuration)
        var fromJson = mapper.Map<DestinationModel>(sourceFromJson);

        Console.WriteLine("\nMapped from JSON (via SourceModel):");
        Console.WriteLine(JsonConvert.SerializeObject(fromJson, Formatting.Indented));

        // 6. Test dynamic mapping
        dynamic dynamicResult = mapper.MapToDynamic(source);

        Console.WriteLine("\nDynamic Result:");
        Console.WriteLine($"Id: {dynamicResult.Id}");
        Console.WriteLine($"Name: {dynamicResult.Name}");
        Console.WriteLine($"EmailAddress: {dynamicResult.EmailAddress}");

        Console.WriteLine("\nPress any key to exit...");
        // Removed Console.ReadKey() to avoid blocking in non-interactive environment
        // Console.ReadKey();
    }
}
