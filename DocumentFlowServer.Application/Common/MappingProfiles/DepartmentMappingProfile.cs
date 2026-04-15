using AutoMapper;
using DocumentFlowServer.Application.Department.Dtos;

namespace DocumentFlowServer.Application.Common.MappingProfiles;

public class DepartmentMappingProfile : Profile
{
    public DepartmentMappingProfile()
    {
        //Get Departments
        CreateMap<Entities.Models.Department, DepartmentDto>();
        
        //CreateNewDepartment
        CreateMap<CreateDepartmentDto, Entities.Models.Department>();
        
        //UpdateDepartment
        CreateMap<UpdateDepartmentDto, Entities.Models.Department>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
        
        //RefreshTokenDto
        CreateMap<DepartmentCleanDto, Entities.Models.Department>();
    }
}