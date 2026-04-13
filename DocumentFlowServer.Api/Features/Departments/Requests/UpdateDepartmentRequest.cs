using System.Text.Json.Serialization;

namespace DocumentFlowServer.Api.Features.Departments.Requests;

public class UpdateDepartmentRequest
{
    [JsonPropertyName("title")]
    public string? Title { get; set; }
    
    [JsonPropertyName("description")]
    public string? Description { get; set; }
    
    [JsonPropertyName("employeesIds")]
    public ICollection<int>? EmployeesIds { get; set; }
}