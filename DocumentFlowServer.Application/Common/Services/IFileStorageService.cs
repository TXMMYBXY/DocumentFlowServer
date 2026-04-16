namespace DocumentFlowServer.Application.Common.Services;

public interface IFileStorageService
{
    Task<string> SaveFileAsync(Stream fileStream, string fileName, string projectFolder);
    Task DeleteFileAsync(string filePath);
}