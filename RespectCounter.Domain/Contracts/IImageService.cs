namespace RespectCounter.Domain.Contracts;

public interface IImageService
{
    Task<string> SaveImageAsync(Stream fileStream, string fileName, string contentType, string folderName);
    Task DeleteImageAsync(string fileIdentifier);
}