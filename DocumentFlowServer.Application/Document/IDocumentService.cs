using DocumentFlowServer.Application.Document.Dtos;

namespace DocumentFlowServer.Application.Document;

public interface IDocumentService
{
    Task<DownloadDocumentDto> DownloadDocumentAsync(int documentId);
    Task UploadDocumentAsync(UploadDocumentDto documentDto);
}