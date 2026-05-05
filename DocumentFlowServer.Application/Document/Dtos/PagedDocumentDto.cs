using DocumentFlowServer.Application.Common;

namespace DocumentFlowServer.Application.Document.Dtos;

public class PagedDocumentDto : PagedData
{
    public ICollection<DocumentDto?> Documents { get; set; }
}