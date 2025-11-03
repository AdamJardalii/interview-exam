using CadabraTest.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "CADABRA CAD Part Analysis API",
        Version = "1.0.0",
        Description = "API for analyzing CAD part files with AI-powered insights"
    });
});

// TODO: Add HTTP client for AI service if you want to use LLM providers (optional)
// builder.Services.AddHttpClient<AIAnalysisService>();

// TODO: Register your services here
// Uncomment and implement the services first:
// builder.Services.AddScoped<ICadProcessingService, CadProcessingService>();
// builder.Services.AddScoped<IAIAnalysisService, AIAnalysisService>();
// builder.Services.AddSingleton<IAnalysisStorageService, AnalysisStorageService>();

// TODO: Add CORS if needed
// builder.Services.AddCors(options => { ... });

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "CADABRA API v1");
        c.RoutePrefix = "swagger";
    });
}

// TODO: Add middleware as needed
// app.UseCors();
app.UseHttpsRedirection();
// app.UseAuthorization(); // Remove if no authentication configured
app.MapControllers();

app.Run();

