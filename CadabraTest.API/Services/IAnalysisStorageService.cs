using CadabraTest.API.Models;

namespace CadabraTest.API.Services;

/// <summary>
/// Storage service interface for analysis results
/// TODO: Implement storage for analysis results (in-memory or database)
/// </summary>
public interface IAnalysisStorageService
{
    Task<AnalysisResponse> SaveAnalysisAsync(AnalysisResponse analysis);
    Task<AnalysisResponse?> GetAnalysisAsync(Guid analysisId);
    Task<bool> DeleteAnalysisAsync(Guid analysisId);
    Task<List<AnalysisResponse>> GetAllAnalysesAsync(string userId);
}

