using DocumentFlowServer.Application.Common;

namespace DocumentFlowServer.Application.Services.User.Dtos;

public class PagedUserDto : PagedData
{
    public ICollection<UserDto>? Users { get; set; }
}