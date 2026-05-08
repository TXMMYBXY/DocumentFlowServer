using DocumentFlowServer.Entities.Enums;

namespace DocumentFlowServer.Application.User;

public class UserFilter
{
    public UserSortField? SortBy { get; set; }
    public bool Descending { get; set; }
    
    public string? FullName { get; set; }
    public string? Email { get; set; }
    public int? DepartmentId { get; set; }
    public Role? Role { get; set; }
    
    public int? PageSize { get; set; }
    public int? PageNumber { get; set; }
}