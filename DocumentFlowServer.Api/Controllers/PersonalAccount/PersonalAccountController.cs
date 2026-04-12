using System.Security.Claims;
using AutoMapper;
using DocumentFlowServer.Api.Controllers.Authorization;
using DocumentFlowServer.Api.Controllers.PersonalAccount.ViewModels;
using DocumentFlowServer.Application.Services.Personal;
using DocumentFlowServer.Application.Services.Personal.Dto;
using Microsoft.AspNetCore.Mvc;

namespace DocumentFlowServer.Api.Controllers.PersonalAccount;

[ApiController]
[Route("api/personal")]
[AuthorizeByRoleId]
public class PersonalAccountController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IPersonalAccountService _personalAccountService;

    public PersonalAccountController(IMapper mapper, IPersonalAccountService personalAccountService)
    {
        _mapper = mapper;
        _personalAccountService = personalAccountService;
    }

    [HttpGet]
    public async Task<ActionResult<GetPersonViewModel>> GetPersonalInfo()
    {
        var personDto = await _personalAccountService
            .GetPersonalInfoAsync(int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value));
        var personViewModel = _mapper.Map<GetPersonViewModel>(personDto);

        return Ok(personViewModel);
    }

    [HttpPatch("change-password")]
    public async Task<ActionResult> ChangePassword([FromBody] ChangePasswordViewModel  changePasswordViewModel)
    {
        var changePasswordDto = _mapper.Map<ChangePasswordDto>(changePasswordViewModel);

        await _personalAccountService.ChangePasswordAsync(
            int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value), changePasswordDto);
        
        return Ok();
    }

    [HttpGet("login-times")]
    public async Task<ActionResult<List<GetLoginTimesViewModel>>> GetLoginTimes()
    {
        var loginTimesDto = await _personalAccountService
            .GetLoginTimesAsync(int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value));
        var loginTimesViewModel = _mapper.Map<List<GetLoginTimesViewModel>>(loginTimesDto);
        
        return Ok(loginTimesViewModel);
    }
}