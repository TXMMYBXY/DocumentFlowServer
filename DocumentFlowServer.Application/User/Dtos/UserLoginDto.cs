namespace DocumentFlowServer.Application.User.Dtos;

/// <summary>
/// Dto для получения полной информации о пользователе
/// </summary>
public class UserLoginDto : UserDto
{
    public string PasswordHash { get; set; }
}