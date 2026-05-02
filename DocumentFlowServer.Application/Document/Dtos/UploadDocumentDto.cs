using DocumentFlowServer.Entities.Enums;

namespace DocumentFlowServer.Application.Document.Dtos;

public class UploadDocumentDto
{
    public string Title { get; set; }
    public int CreatedBy { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DocumentType DocumentType { get; set; }
    public int TemplateId { get; set; }
    public string FileName { get; set; }
    public long FileLength { get; set; }
    public Stream FileStream { get; set; }
}