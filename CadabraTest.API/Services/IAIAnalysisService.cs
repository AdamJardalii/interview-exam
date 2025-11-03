using CadabraTest.API.Models;

namespace CadabraTest.API.Services;

/// <summary>
/// Service interface for analysis of CAD parts
/// Note: AI/LLM integration is optional - you can return basic analysis without LLM
/// </summary>
public interface IAIAnalysisService
{
    /// <summary>
    /// Generate analysis and insights for a CAD part
    /// </summary>
    /// <param name="partMetadata">The part metadata to analyze</param>
    /// <returns>Analysis with insights and recommendations</returns>
    Task<AIAnalysis> GenerateAnalysisAsync(PartMetadata partMetadata);
}

