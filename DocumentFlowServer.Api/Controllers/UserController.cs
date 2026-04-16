using AutoMapper;
using DocumentFlowServer.Api.Features.User.Requests;
using DocumentFlowServer.Api.Features.User.Responses;
using DocumentFlowServer.Application.User;
using DocumentFlowServer.Application.User.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DocumentFlowServer.Api.Controllers;

[ApiController]
[Route("api/user")]
[Authorize(Policy = "AdminOnly")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly IMapper _mapper;
    private readonly IUserService _userService;

    public UserController(
        ILogger<UserController> logger,
        IMapper mapper,
        IUserService userService)
    {
        _logger = logger;
        _mapper = mapper;
        _userService = userService;
    }
    
    [HttpGet]
    public async Task<ActionResult<PagedUserResponse>> GetAllUser([FromQuery] GetUsersRequest request)
    {
        _logger.LogInformation("Request returns users");
        
        var filter = _mapper.Map<GetUsersRequest, UserFilter>(request);
        
        var result = await _userService.GetUsersAsync(filter);
        
        var response = _mapper.Map<PagedUserResponse>(result);

        return Ok(response);
    }
    
    [HttpPost]
    public async Task<ActionResult> CreateNewUser([FromBody] CreateUserRequest request)
    {
        _logger.LogInformation("Request creates user");
        
        var userDto = _mapper.Map<CreateUserDto>(request);

        await _userService.CreateUserAsync(userDto);

        return Created();
    }
    
    [HttpPatch("{userId}/user-info")]
    public async Task<ActionResult> UpdateUserPartialAsync([FromRoute] int userId, [FromBody] UpdateUserRequest request)
    {
        _logger.LogInformation("Request updates user");
        
        var userDto = _mapper.Map<UpdateUserInfoDto>(request);

        await _userService.UpdateUserInfoAsync(userId, userDto);

        return Ok();
    }

    [HttpPatch("{userId}/reset-password")]
    public async Task<ActionResult> ResetPasswordAsync([FromRoute] int userId, SetUserPasswordRequest request)
    {
        _logger.LogInformation("Request sets new password");
        
        var resetPasswordDto = _mapper.Map<SetUserPasswordDto>(request);
        
        await _userService.SetUserPasswordAsync(userId, resetPasswordDto);

        return Ok();
    }
    
    [HttpPatch("{userId}/change-status")]
    public async Task<ActionResult<bool>> ChangeUserStatusByidAsync([FromRoute] int userId)
    {
        _logger.LogInformation("Request changes user`s status");
        
        var status = await _userService.ChangeUserStatusAsync(userId);

        return Ok(status);
    }

    [HttpDelete("{userId:int}")]
    public async Task<ActionResult> DeleteUser([FromRoute] int userId)
    {
        await _userService.DeleteUserAsync(userId);

        return Ok();
    }
}