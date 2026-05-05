using System.Text.Json.Serialization;
using DocumentFlowServer.Application.Document.Dtos;

namespace DocumentFlowServer.Api.Features.Document.Responses;

public class PagedDocumentResponse
{
    [JsonPropertyName("documents")]
    public ICollection<DocumentDto>? Documents { get; set; }
    
    [JsonPropertyName("totalCount")]
    public int TotalCount { get; set; }

    [JsonPropertyName("pageSize")]
    public int PageSize { get; set; }

    [JsonPropertyName("currentPage")]
    public int CurrentPage { get; set; }

    [JsonPropertyName("totalPages")]
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
}