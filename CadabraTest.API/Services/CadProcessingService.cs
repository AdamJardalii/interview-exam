using System.Text.Json;
using CadabraTest.API.Models;

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
        // TODO: Implement metadata extraction from CAD file
        // 
        // You can:
        // 1. Parse JSON mock data files (see SampleData/mock_part_data.json)
        // 2. Extract metadata: part name, dimensions, material, mass properties
        // 3. Handle file validation and errors
        // 4. Return PartMetadata object
        
        throw new NotImplementedException("Implement metadata extraction from CAD file");
    }
}

