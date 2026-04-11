using DocumentFlowServer.Application.Repository.Token.Dto;

namespace DocumentFlowServer.Api.Controllers.Authorization.ViewModels;

public class RefreshTokenToLoginResponseViewModel
{
    public bool IsAllowed { get; set; }
    public RefreshTokenDto RefreshToken { get; set; }
}
