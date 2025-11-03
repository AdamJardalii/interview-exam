using CadabraTest.API.Models;

namespace CadabraTest.API.Services;

/// <summary>
/// Service interface for processing CAD part files
/// </summary>
public interface ICadProcessingService
{
    /// <summary>
    /// Extract metadata from a CAD part file
    /// </summary>
    /// <param name="fileStream">Stream containing the CAD file</param>
    /// <param name="fileName">Name of the file</param>
    /// <returns>Part metadata</returns>
    Task<PartMetadata> ExtractMetadataAsync(Stream fileStream, string fileName);
}

