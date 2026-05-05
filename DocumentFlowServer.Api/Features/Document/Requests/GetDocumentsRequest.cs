using DocumentFlowServer.Application.Document;

namespace DocumentFlowServer.Api.Features.Document.Requests;

public class GetDocumentsRequest
{
    public DocumentSortField? SortByField { get; set; }
    public bool Descending { get; set; }
    
    public string? Title { get; set; }
    public int? TemplateId { get; set; }
    public DateTime? CreatedAtEarlier { get; set; }
    public DateTime? CreatedAtLater { get; set; }
    
    public int? PageSize { get; set; }
    public int? PageNumber { get; set; }
}