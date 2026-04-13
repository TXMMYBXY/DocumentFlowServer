using System.Collections.Generic;
using DocumentFlowServer.Application.Common;

namespace DocumentFlowServer.Application.User.Dtos;

public class PagedUserDto : PagedData
{
    public ICollection<UserDto>? Users { get; set; }
}