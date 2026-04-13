using System.Text.Json.Serialization;
using DocumentFlowServer.Application.Services.Department.Dto;

namespace DocumentFlowServer.Api.Controllers.Department.ViewModels;

public class GetDepartmentViewModel
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    
    [JsonPropertyName("title")]
    public string Title { get; set; }
    
    [JsonPropertyName("description")]
    public string? Description { get; set; }
    
    [JsonPropertyName("employees")]
    public virtual List<EmployeeDto> Employees { get; set; }
}