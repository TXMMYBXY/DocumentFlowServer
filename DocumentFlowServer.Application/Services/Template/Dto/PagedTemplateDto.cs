namespace DocumentFlowServer.Application.Services.Template.Dto;

public class PagedTemplateDto
{
    public List<GetTemplateDto> Templates { get; set; }
    public int TotalCount { get; set; }
    public int PageSize { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
}
