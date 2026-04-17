using System.Security.Claims;
using AutoMapper;
using DocumentFlowServer.Api.Authorization.Policies;
using DocumentFlowServer.Api.Features.Personal.Requests;
using DocumentFlowServer.Api.Features.Personal.Responses;
using DocumentFlowServer.Application.Personal;
using DocumentFlowServer.Application.Personal.Dtos;
using DocumentFlowServer.Infrastructure.Common.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DocumentFlowServer.Api.Controllers;

[ApiController]
[Route("api/personal")]
[Authorize(Policy = Policy.All)]
public class PersonalAccountController : ControllerBase
{
    private readonly ILogger<PersonalAccountService> _logger;
    private readonly IMapper _mapper;
    private readonly IPersonalAccountService _personalAccountService;

    public PersonalAccountController(
        ILogger<PersonalAccountService> logger,
        IMapper mapper,
        IPersonalAccountService personalAccountService)
    {
        _logger = logger;
        _mapper = mapper;
        _personalAccountService = personalAccountService;
    }
    
    [HttpGet]
    public async Task<ActionResult<PersonResponse>> GetPersonalInfo()
    {
        var personDto = await _personalAccountService.GetPersonalInfoAsync(
            int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value));
        
        var personViewModel = _mapper.Map<PersonResponse>(personDto);

        return Ok(personViewModel);
    }

    [HttpPatch("change-password")]
    public async Task<ActionResult> ChangePassword([FromBody] ChangePasswordRequest  request)
    {
        var changePasswordDto = _mapper.Map<ChangePasswordDto>(request);
        
        await _personalAccountService.ChangePasswordAsync(
            int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value), changePasswordDto);
        
        return Ok();
    }

    [HttpGet("login-times")]
    public async Task<ActionResult<List<LoginTimeResponse>>> GetLoginTimes()
    {
        var loginTimesDto = await _personalAccountService.GetLoginTimesAsync(
            int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value));
        
        var loginTimesViewModel = _mapper.Map<List<LoginTimeResponse>>(loginTimesDto);
        
        return Ok(loginTimesViewModel);
    }
}