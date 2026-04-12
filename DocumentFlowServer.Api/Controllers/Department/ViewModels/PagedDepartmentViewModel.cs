using System.Text.Json.Serialization;

namespace DocumentFlowServer.Api.Controllers.Department.ViewModels;

public class PagedDepartmentViewModel : PagedData
{
    [JsonPropertyName("departments")]
    public List<GetDepartmentViewModel> Departments { get; set; }
}
