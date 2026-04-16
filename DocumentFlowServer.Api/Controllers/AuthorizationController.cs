using AutoMapper;
using DocumentFlowServer.Api.Features.Authorization.Requests;
using DocumentFlowServer.Api.Features.Authorization.Responses;
using DocumentFlowServer.Application.Account;
using DocumentFlowServer.Application.Account.RequestDto;
using Microsoft.AspNetCore.Mvc;

namespace DocumentFlowServer.Api.Controllers;

[ApiController]
[Route("api/authorization")]
public class AuthorizationController : ControllerBase
{
    private readonly ILogger<AuthorizationController> _logger;
    private readonly IMapper _mapper;
    private readonly IAccountService _accountService;

    public AuthorizationController(
        ILogger<AuthorizationController> logger,
        IMapper mapper,
        IAccountService accountService)
    {
        _logger = logger;
        _mapper = mapper;
        _accountService = accountService;
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login(LoginRequest request)
    {
        _logger.LogInformation("request for login");
        
        var loginRequest = _mapper.Map<LoginRequestDto>(request);

        var loginResponse = await _accountService.LoginAsync(loginRequest);
        
        var response = _mapper.Map<LoginResponse>(loginResponse);
        
        return Ok(response);
    }

    [HttpPost("request-for-access")]
    public async Task<ActionResult<LoginRefreshResponse>> LoginByRefreshToken(
        [FromBody] RefreshTokenLoginRequest request)
    {
        _logger.LogInformation("request for login by refresh token");
        
        var responseDto = await _accountService.LoginByRefreshTokenAsync(request.RefreshToken);
        
        var response = _mapper.Map<LoginRefreshResponse>(responseDto);
        
        return Ok(response);
    }

    [HttpPost("access")]
    public async Task<ActionResult<AccessTokenResponse>> AccessToken([FromBody] AccessTokenRequest request)
    {
        _logger.LogInformation("request for new access token");
        
        var responseDto = await _accountService.GetNewAccessTokenAsync(request.RefreshToken);

        var response = _mapper.Map<AccessTokenResponse>(responseDto);

        return Ok(response);
    }
    
    [HttpPost("refresh")]
    public async Task<ActionResult<RefreshTokenResponse>> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        _logger.LogInformation("request for new refresh token");

        var responseDto = await _accountService.GetNewRefreshTokenAsync(request.RefreshToken);

        var response = _mapper.Map<RefreshTokenResponse>(responseDto);

        return Ok(response);
    }
}