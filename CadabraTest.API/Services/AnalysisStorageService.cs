using CadabraTest.API.Models;
using CadabraTest.API.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text;


namespace CadabraTest.API.Services;

/// <summary>
/// Storage service implementation for analysis results
/// TODO: Implement storage for analysis results (in-memory or database)
/// </summary>
public class AnalysisStorageService : IAnalysisStorageService
{
    private readonly ILogger<AnalysisStorageService> _logger;
    private readonly AppDbContext _dbContext;

    private readonly Dictionary<Guid, AnalysisResponse> _storage = new();


    public AnalysisStorageService(ILogger<AnalysisStorageService> logger,AppDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task<AnalysisResponse> SaveAnalysisAsync(AnalysisResponse analysis)
    {
        if (!_dbContext.Analyses.Any(a => a.AnalysisId == analysis.AnalysisId))
        {
            analysis.Id = Guid.NewGuid();
            analysis.CreatedAt = DateTime.UtcNow;

            _dbContext.Analyses.Add(analysis);
            await _dbContext.SaveChangesAsync();
        }
        else
        {
            _dbContext.Analyses.Update(analysis);
            await _dbContext.SaveChangesAsync();
        }

        return analysis;
    }


    // public Task<AnalysisResponse> SaveAnalysisAsync(AnalysisResponse analysis)
    // {
    //     _storage[analysis.AnalysisId] = analysis;
    //     return Task.FromResult(analysis);
    //     // TODO: Implement storage for analysis results
    //     // You can use:
    //     // - In-memory storage (Dictionary/ConcurrentDictionary)
    //     // - Database (SQLite, SQL Server, etc.)
    //     // - Other storage solutions
        
    // }

    // public Task<AnalysisResponse?> GetAnalysisAsync(Guid analysisId)
    // {
    //     _storage.TryGetValue(analysisId, out var analysis);
    //     return Task.FromResult(analysis);
    // }
    public async Task<AnalysisResponse?> GetAnalysisAsync(Guid analysisId)
    {
        return await _dbContext.Analyses
            .Include(a => a.AIAnalysis)
            .FirstOrDefaultAsync(a => a.AnalysisId == analysisId);
    }


    // public Task<bool> DeleteAnalysisAsync(Guid analysisId)
    // {
    //     var removed = _storage.Remove(analysisId);
    //     return Task.FromResult(removed);
    // }

    public async Task<bool> DeleteAnalysisAsync(Guid analysisId)
    {
        var analysis = await _dbContext.Analyses.FirstOrDefaultAsync(a => a.AnalysisId == analysisId);
        if (analysis == null) return false;

        _dbContext.Analyses.Remove(analysis);
        await _dbContext.SaveChangesAsync();
        return true;
    }
    // public Task<List<AnalysisResponse>> GetAllAnalysesAsync()
    // {
    //     var all = _storage.Values.ToList();
    //     return Task.FromResult(all);
    // }

    public async Task<List<AnalysisResponse>> GetAllAnalysesAsync(string userId)
    {
        return await _dbContext.Analyses
            .Include(a => a.AIAnalysis)
            .Where(a => a.UserId == userId)
            .ToListAsync();
    }
}
