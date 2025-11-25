using System.Text.Json;
using CadabraTest.API.Models;
using System.Diagnostics;

namespace CadabraTest.API.Services;

/// <summary>
/// Service for processing CAD part files and extracting metadata
/// TODO: Implement this service to extract metadata from CAD files
/// </summary>
public class CadProcessingService : ICadProcessingService
{
    private readonly ILogger<CadProcessingService> _logger;
    private readonly IConfiguration _configuration;

    public CadProcessingService(ILogger<CadProcessingService> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    public async Task<PartMetadata> ExtractMetadataAsync(Stream fileStream, string fileName)
    {
        try
        {
            // Reset stream in case it was read before
            if (fileStream.CanSeek)
                fileStream.Position = 0;

            // Parse the JSON document
            using var doc = await JsonDocument.ParseAsync(fileStream);
            var root = doc.RootElement;

            // Map the JSON to your PartMetadata template
            var metadata = MapJsonToPartMetadata(root);

            // Attach filename (optional)
            metadata.FileName = fileName;

            // Debug: print metadata
            var jsonString = JsonSerializer.Serialize(metadata, new JsonSerializerOptions { WriteIndented = true });

            return metadata;
        }
        catch (JsonException ex)
        {
            throw new Exception("Invalid JSON format.", ex);
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to extract metadata from CAD JSON.", ex);
        }
    }


    // public async Task<PartMetadata> ExtractMetadataAsync(Stream fileStream, string fileName)
    // {
    //     try
    //     {
    //         // Deserialize JSON directly from stream
    //         var metadata = await JsonSerializer.DeserializeAsync<PartMetadata>(fileStream);

    //         if (metadata == null)
    //             throw new Exception("Failed to parse JSON.");

    //         // Optionally attach filename
    //         metadata.FileName = fileName;

    //         var jsonString = JsonSerializer.Serialize(metadata, new JsonSerializerOptions { WriteIndented = true });
    //         Console.WriteLine(jsonString);
    //         return metadata;
    //     }
    //     catch (JsonException ex)
    //     {
    //         throw new Exception("Invalid JSON format.", ex);
    //     }
    //     // TODO: Implement metadata extraction from CAD file
    //     // 
    //     // You can:
    //     // 1. Parse JSON mock data files (see SampleData/mock_part_data.json)
    //     // 2. Extract metadata: part name, dimensions, material, mass properties
    //     // 3. Handle file validation and errors
    //     // 4. Return PartMetadata object
        
    //     throw new NotImplementedException("Implement metadata extraction from CAD file");
    // }

    public static PartMetadata MapJsonToPartMetadata(JsonElement root)
    {
        var metadata = new PartMetadata
        {
            FileName = root.GetProperty("fileName").GetString() ?? string.Empty,
            PartName = root.GetProperty("partName").GetString() ?? string.Empty
        };

        if (root.TryGetProperty("dimensions", out var dim))
        {
            metadata.Dimensions = new Dimensions
            {
                Length = dim.GetProperty("length").GetDouble(),
                Width = dim.GetProperty("width").GetDouble(),
                Height = dim.GetProperty("height").GetDouble(),
                Units = dim.GetProperty("units").GetString() ?? "mm"
            };
        }

        if (root.TryGetProperty("material", out var mat))
            metadata.Material = mat.GetProperty("name").GetString() ?? string.Empty;

        if (root.TryGetProperty("massProperties", out var mass))
        {
            metadata.Mass = mass.GetProperty("mass").GetDouble();
            metadata.Volume = mass.GetProperty("volume").GetDouble();
        }

        if (root.TryGetProperty("customProperties", out var customProps))
        {
            metadata.CustomProperties = new Dictionary<string, string>();
            foreach (var prop in customProps.EnumerateObject())
                metadata.CustomProperties[prop.Name] = prop.Value.ToString() ?? string.Empty;
        }

        return metadata;
    }

}

