namespace DocumentFlowServer.Worker.Client.Api.Dto;

public class UploadDocumentDto
{
    public string Title { get; set; } = null!;
    public int CreatedBy { get; set; }
    public int TemplateId { get; set; }

    public string FileName { get; set; } = null!;
    public byte[] Content { get; set; } = null!;
}