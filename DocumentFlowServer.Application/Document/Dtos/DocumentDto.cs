using DocumentFlowServer.Application.User.Dtos;
using DocumentFlowServer.Entities.Enums;

namespace DocumentFlowServer.Application.Document.Dtos;

public class DocumentDto
{
    public string Title { get; set; }
    public string FilePath { get; set; }
    public UserCleanDto User { get; set; }
    public DateTime CreatedAt { get; set; }
    public DocumentType Type { get; set; }
    public int TemplateId { get; set; }
}