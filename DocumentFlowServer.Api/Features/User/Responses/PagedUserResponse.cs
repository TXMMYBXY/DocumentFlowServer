using System.Text.Json.Serialization;
using DocumentFlowServer.Application.User.Dtos;

namespace DocumentFlowServer.Api.Features.User.Responses;

public class PagedUserResponse
{
    [JsonPropertyName("users")]
    public ICollection<UserDto>? Users { get; set; }
    
    [JsonPropertyName("totalCount")]
    public int TotalCount { get; set; }

    [JsonPropertyName("pageSize")]
    public int PageSize { get; set; }

    [JsonPropertyName("currentPage")]
    public int CurrentPage { get; set; }

    [JsonPropertyName("totalPages")]
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
}