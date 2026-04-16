using DocumentFlowServer.Application.Common;

namespace DocumentFlowServer.Application.Template.Dtos;

public class PagedTemplateDto : PagedData
{
    public ICollection<TemplateDto> Templates { get; set; }
}