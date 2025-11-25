using Microsoft.AspNetCore.Mvc;
using CadabraTest.API.Models;
using CadabraTest.API.Services;
using CadabraTest.API.Helpers;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;


namespace CadabraTest.API.Controllers;

/// <summary>
/// CAD Part Analysis API Controller
/// TODO: Implement all endpoints
/// </summary>
[ApiController]
[Route("api/cad")]
public class CadController : ControllerBase
{
    private readonly ILogger<CadController> _logger;
    // TODO: Inject your services here
    private readonly ICadProcessingService _cadProcessingService;
    private readonly IAIAnalysisService _aiAnalysisService;
    private readonly IAnalysisStorageService _storageService;
    // private readonly IConfiguration _configuration;

    public CadController(ILogger<CadController> logger,ICadProcessingService CadProcessingService,IAIAnalysisService AIAnalysisService,IAnalysisStorageService AnalysisStorageService)
    {
        _logger = logger;
        _cadProcessingService = CadProcessingService;
        _aiAnalysisService = AIAnalysisService;
        _storageService = AnalysisStorageService;
        // TODO: Add service parameters and initialize them
    }

    /// <summary>
    /// Analyze a CAD part file
    /// TODO: Implement this endpoint
    /// </summary>
    [HttpPost("analyze")]
    [Authorize]
    [ProducesResponseType(typeof(AnalysisResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AnalyzePart(
        IFormFile? file,
        [FromForm] bool includeAIAnalysis = true)
    {
        var validator = new FileValidator(new FileValidationOptions());
        var result = validator.Validate(file);

        if (!result.IsValid)
            return BadRequest(result.Error);
        
        using var stream = file!.OpenReadStream();

        var partMetadata = await _cadProcessingService.ExtractMetadataAsync(stream, file.FileName);

        AIAnalysis? aiAnalysis = await _aiAnalysisService.GenerateAnalysisAsync(partMetadata);
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

         var analysisResponse = new AnalysisResponse
        {
            AnalysisId = Guid.NewGuid(),
            Status = "completed",
            PartMetadata = partMetadata,
            AIAnalysis = aiAnalysis,
            CreatedAt = DateTime.UtcNow,
            CompletedAt = DateTime.UtcNow,
            UserId = userId!
        };

        await _storageService.SaveAnalysisAsync(analysisResponse);

        return Ok(analysisResponse);


        // TODO: Implement this endpoint
        // 1. Validate the uploaded file (size, extension)
        // 2. Extract metadata from the CAD file using CadProcessingService
        // 3. If includeAIAnalysis is true, generate analysis using AIAnalysisService (optional - can be basic analysis)
        // 4. Save the analysis using AnalysisStorageService
        // 5. Return the analysis response
        
        return BadRequest(new { error = " Hello World!" });
    }

    /// <summary>
    /// Get analysis result by ID
    /// TODO: Implement this endpoint
    /// </summary>
    [HttpGet("analysis/{analysisId}")]
    [Authorize] 
    [ProducesResponseType(typeof(AnalysisResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetAnalysis(Guid analysisId)
    {
          // 1. Retrieve analysis by ID
        var analysis = await _storageService.GetAnalysisAsync(analysisId);

        // 2. Return 404 if not found
        if (analysis == null)
        {
            return NotFound(new { error = $"Analysis with ID {analysisId} not found." });
        }
        
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // 4. Check if the analysis belongs to the authenticated user
        if (analysis.UserId != userId)
            return Forbid(); // 403 Forbidden

        // 3. Return 200 OK with the analysis
        return Ok(analysis);
    }

    [HttpGet("all")]
    [Authorize]
    public async Task<IActionResult> GetAllAnalyses()
    {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
            return Unauthorized();
        var analyses = await _storageService.GetAllAnalysesAsync(userId);
        return Ok(new
        {
            count = analyses.Count,
            items = analyses
        });
    }

    [HttpDelete("analysis/{analysisId}")]
    [Authorize] 
    [ProducesResponseType(typeof(AnalysisResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> DeleteAnalysis(Guid analysisId)
    {

        var analysis = await _storageService.GetAnalysisAsync(analysisId);

        if (analysis == null)
            return NotFound(new { error = "Analysis not found", analysisId });

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
            return Unauthorized();

        if (analysis.UserId != userId)
            return Forbid(); 

        var deleted = await _storageService.DeleteAnalysisAsync(analysisId);        

        return Ok(new { message = "Analysis deleted successfully", analysisId});
    }

    /// <summary>
    /// Health check endpoint
    /// TODO: Implement health check
    /// </summary>
    [HttpGet("health")]
    [ProducesResponseType(typeof(HealthResponse), StatusCodes.Status200OK)]
    public IActionResult HealthCheck()
    {
        // TODO: Implement health check
        // Return basic health status
        
        return Ok(new HealthResponse
        {
            Status = "healthy",
            Timestamp = DateTime.UtcNow,
            Version = "1.0.0"
        });
    }

    // TODO: Add at least 2 additional endpoints of your choice
    // Examples:
    // - GET /api/cad/analyses - List all analyses
    // - DELETE /api/cad/analysis/{id} - Delete an analysis
    // - GET /api/cad/statistics - Get statistics about analyses
    // etc.
}

