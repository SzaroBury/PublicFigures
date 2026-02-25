using RespectCounter.Application.Shared.Contracts;

namespace RespectCounter.Infrastructure.Services;

public class LocalImageService : IImageService
{
    private readonly string _basePath;

    public LocalImageService(string basePath)
    {
        _basePath = Path.Combine(basePath, "images"); 
        Directory.CreateDirectory(_basePath); 
    }

    public Task DeleteImageAsync(string fileIdentifier)
    {
        var fullPath = Path.Combine(_basePath, fileIdentifier);

        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }

        return Task.CompletedTask;
    }

    public async Task<string> SaveImageAsync(Stream fileStream, string fileName, string contentType, string folderName)
    {
        var targetFolder = Path.Combine(_basePath, folderName);
        Directory.CreateDirectory(targetFolder);

        var fullPath = Path.Combine(targetFolder, fileName);

        using (var file = new FileStream(fullPath, FileMode.Create))
        {
            await fileStream.CopyToAsync(file);
        }

        return $"/images/{folderName}/{fileName}"; 
    }

}