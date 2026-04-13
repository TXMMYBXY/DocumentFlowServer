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
        var loginRequest = _mapper.Map<LoginRequestDto>(request);

        var loginResponse = await _accountService.LoginAsync(loginRequest);
        
        var response = _mapper.Map<LoginResponse>(loginResponse);
        
        return Ok(response);
    }
}