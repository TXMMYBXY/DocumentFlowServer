using DocumentFlowServer.Application.Template.Dtos;
using DocumentFlowServer.Application.User.Dtos;
using DocumentFlowServer.Entities.Enums;

namespace DocumentFlowServer.Application.Document.Dtos;

public class DocumentDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string FilePath { get; set; }
    public UserCleanDto User { get; set; }
    public DateTime CreatedAt { get; set; }
    public TemplateType Type { get; set; }
    public TemplateClearDto Template { get; set; }
}