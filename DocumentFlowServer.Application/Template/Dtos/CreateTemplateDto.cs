namespace DocumentFlowServer.Application.Template.Dtos;

public class CreateTemplateDto
{
    public string Title { get; set; }
    public bool IsActive { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public string FileName { get; set; }
    public long FileLength { get; set; }
    public Stream FileStream { get; set; }
}