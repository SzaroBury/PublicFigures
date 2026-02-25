using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RespectCounter.Application.Shared.Contracts;

namespace RespectCounter.Infrastructure.Services;

public class DatabaseInitializer : IDatabaseInitializer
{
    private readonly RespectDbContext _context;
    private readonly ILogger<DatabaseInitializer> _logger;
    private readonly IImageService _imageService;
    private readonly string _seedAssetsPath;
    
    private static readonly Dictionary<string, string> PeopleToSeed = new()
    {
        { "Lewandowski", "person_lewandowski.jpg" },
        { "Kubica", "person_kubica.jpg" }
    };

    public DatabaseInitializer(RespectDbContext context, ILogger<DatabaseInitializer> logger, IImageService imageService, string? seedAssetsPath = null)
    {
        _context = context;
        _logger = logger;
        _imageService = imageService;
        _seedAssetsPath = seedAssetsPath ?? "../SeedData/images";
    }

    public async Task InitializeAsync()
    {
        _logger.LogInformation("Checking for pending EF Core migrations...");

        var pendingMigrations = await _context.Database.GetPendingMigrationsAsync();
        if (pendingMigrations.Any())
        {
            _logger.LogInformation("Pending migrations found. Applying migrations...");
            await _context.Database.MigrateAsync();
            _logger.LogInformation("Migrations applied successfully.");
        }
        else
        {
            _logger.LogInformation("No pending migrations found. Database is up-to-date.");
        }

        _logger.LogInformation("Starting image seeding...");
        await SeedPersonImagesAsync(); 
        _logger.LogInformation("Database initialization complete.");
    }
    
    private async Task SeedPersonImagesAsync()
    {
        foreach (var entry in PeopleToSeed)
        {
            var lastName = entry.Key;
            var filePath = Path.Combine("persons", entry.Value) ;
            
            var person = await _context.Persons.SingleOrDefaultAsync(p => p.LastName == lastName);

            if (person != null && string.IsNullOrEmpty(person.AvatarUrl))
            {
                person.AvatarUrl = await SeedSingleImage(person.Id, filePath, "persons");
            }
        }
    }

    private async Task<string> SeedSingleImage(Guid personId, string filePath, string targetFolder)
    {
        var sourcePath = Path.Combine(_seedAssetsPath, filePath);
        if (!File.Exists(sourcePath))
        {
            Console.WriteLine($"WARNING: Seed asset not found at {sourcePath}");
            return string.Empty;
        }

        var uniqueFileName = "person_" + personId.ToString() + Path.GetExtension(filePath);

        using (var stream = File.OpenRead(sourcePath))
        {
            var contentType = filePath.EndsWith(".jpg") ? "image/jpeg" : "image/png";

            return await _imageService.SaveImageAsync(
                stream,
                uniqueFileName,
                contentType,
                targetFolder
            );
        }
    }
}