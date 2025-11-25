using CadabraTest.API.Helpers;

namespace CadabraTest.API.Helpers;

public class FileValidationOptions
{
    public long MaxSize { get; set; } = 5 * 1024 * 1024;

    public string[] AllowedExtensions { get; set; } =
    {
        ".json",
        ".csv"
    };

    public string[] AllowedMimeTypes { get; set; } =
    {
        "application/json",
        "text/csv",
        "application/vnd.ms-excel" 
    };
}
