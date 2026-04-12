using System.Text.Json.Serialization;
using DocumentFlowServer.Application.Repository.Role.Dto;

namespace DocumentFlowServer.Api.Controllers.PersonalAccount.ViewModels;

public class GetPersonViewModel
{
    [JsonPropertyName("fullName")]
    public string FullName { get; set; }
    
    [JsonPropertyName("email")]
    public string Email { get; set; }
    
    [JsonPropertyName("department")]
    public string Department { get; set; }
    
    [JsonPropertyName("role")]
    public RoleEntity Role { get; set; }
}