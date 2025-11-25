using CadabraTest.API.Models;

namespace CadabraTest.API.Services;

/// <summary>
/// Service for generating analysis of CAD parts
/// TODO: Optional - Implement AI/LLM integration if desired
/// Note: AI integration is optional - you can return basic analysis without LLM
/// </summary>
public class AIAnalysisService : IAIAnalysisService
{
    private readonly ILogger<AIAnalysisService> _logger;
    private readonly IConfiguration _configuration;
    private readonly HttpClient? _httpClient;

    public AIAnalysisService(ILogger<AIAnalysisService> logger, IConfiguration configuration, HttpClient? httpClient = null)
    {
        _logger = logger;
        _configuration = configuration;
        _httpClient = httpClient;
    }

    public async Task<AIAnalysis> GenerateAnalysisAsync(PartMetadata partMetadata)
    {
            var analysis = new AIAnalysis();

            // Example rules:
            if (partMetadata.Dimensions != null)
            {
                if (partMetadata.Dimensions.Length > 100 || partMetadata.Dimensions.Width > 100)
                    analysis.Insights.Add("This part is relatively large. Consider material strength.");
                else
                    analysis.Insights.Add("Part dimensions are moderate.");
            }

            if (!string.IsNullOrEmpty(partMetadata.Material))
            {
                if (partMetadata.Material.Contains("Aluminum"))
                    analysis.Recommendations.Add("Aluminum is lightweight; check for load requirements.");
                else
                    analysis.Recommendations.Add("Review material for strength and cost.");
            }

            if (partMetadata.Mass.HasValue && partMetadata.Mass > 5.0)
                analysis.Insights.Add("Part is heavy; consider weight optimization.");
            else
                analysis.Insights.Add("Mass is within normal range.");

            analysis.Summary = "Basic rule-based analysis completed.";

            return analysis;
            // return Task.FromResult(analysis);
        //
        // You can:
        // 1. Return basic analysis based on part metadata (no LLM required)
        // 2. Optional: Integrate with OpenAI/Anthropic if you want
        // 3. Generate insights about the CAD part (material, dimensions, etc.)
        // 4. Return AIAnalysis object with summary, insights, and recommendations
        
        // Example: Basic analysis without LLM
        // return new AIAnalysis
        // {
        //     Summary = $"Analysis of {partMetadata.PartName}",
        //     Insights = new List<string> { "Basic analysis - implement to add insights" },
        //     Recommendations = new List<string> { "Add recommendations here" }
        // };
    }
}

