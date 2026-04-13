using AutoMapper;
using DocumentFlowServer.Api.Features.User.Requests;
using DocumentFlowServer.Api.Features.User.Responses;
using DocumentFlowServer.Application.User;
using DocumentFlowServer.Application.User.Dtos;

namespace DocumentFlowServer.Api.Features.User;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        //Get Users
        CreateMap<GetUsersRequest, UserFilter>();
        CreateMap<PagedUserDto, PagedUserResponse>();
        
        //Create Users
        CreateMap<CreateUserRequest, CreateUserDto>();
        
        //Update Users
        CreateMap<UpdateUserRequest, UpdateUserInfoDto>();
        CreateMap<SetUserPasswordRequest, SetUserPasswordDto>();
    }
}