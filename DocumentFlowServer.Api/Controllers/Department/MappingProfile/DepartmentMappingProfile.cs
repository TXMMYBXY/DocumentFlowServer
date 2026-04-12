using AutoMapper;
using DocumentFlowServer.Api.Controllers.Department.ViewModels;
using DocumentFlowServer.Application.Services.Department.Dto;

namespace DocumentFlowServer.Api.Controllers.Department.MappingProfile;

public class DepartmentMappingProfile : Profile
{
    public DepartmentMappingProfile()
    {
        //Profiles for GET        
        CreateMap<GetDepartmentDto, GetDepartmentViewModel>().ReverseMap();

        CreateMap<PagedDepartmentDto, PagedDepartmentViewModel>().ReverseMap();

        CreateMap<Entities.Models.Department, GetDepartmentDto>()
            .ForMember(dest => dest.Employees, opt => opt.MapFrom(src => src.Employees))
            .ReverseMap();

        CreateMap<Entities.Models.AboutUserModels.User, EmployeeDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role));


        //Profiles for POST
        CreateMap<CreateDepartmentViewModel, CreateDepartmentDto>();

        CreateMap<CreateDepartmentDto, Entities.Models.Department>();

        //Profiles for PUT
        CreateMap<UpdateDepartmentViewModel, UpdateDepartmentDto>();

        CreateMap<UpdateDepartmentDto, Entities.Models.Department>();
    }
}