namespace DocumentFlowServer.Api.Features.Departments.Requests;

public class GetDepartmentsRequest
{
    public string? Title { get; set; }
    
    public int? PageSize { get; set; }
    public int? PageNumber { get; set; }
}