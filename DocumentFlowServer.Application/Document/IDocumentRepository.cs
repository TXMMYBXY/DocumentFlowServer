using DocumentFlowServer.Application.Common.Reposiroty;
using DocumentFlowServer.Application.Document.Dtos;

namespace DocumentFlowServer.Application.Document;

public interface IDocumentRepository : IBaseRepository<Entities.Models.Document>
{
    Task<DocumentDto> GetFilledDocumentByIdAsync(int documentId);
}