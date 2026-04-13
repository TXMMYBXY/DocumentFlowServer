using DocumentFlowServer.Application.Repository.Role.Dto;

namespace DocumentFlowServer.Api.Controllers.User.ViewModels;

/// <summary>
/// ViewModel со всей информацией о пользователе
/// </summary>
public class GetUserViewModel
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public bool IsActive { get; set; }
    public string Department { get; set; }
    public virtual RoleEntity Role { get; set; }
}