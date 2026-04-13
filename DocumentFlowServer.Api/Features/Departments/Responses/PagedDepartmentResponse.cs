using System.Text.Json.Serialization;
using DocumentFlowServer.Application.Department.Dtos;

namespace DocumentFlowServer.Api.Features.Departments.Responses;

public class PagedDepartmentResponse
{
    [JsonPropertyName("departments")]
    public ICollection<DepartmentDto>? Departments { get; set; }
    
    [JsonPropertyName("totalCount")]
    public int TotalCount { get; set; }

    [JsonPropertyName("pageSize")]
    public int PageSize { get; set; }

    [JsonPropertyName("currentPage")]
    public int CurrentPage { get; set; }

    [JsonPropertyName("totalPages")]
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
}