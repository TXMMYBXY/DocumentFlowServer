using AutoMapper;
using DocumentFlowServer.Api.Features.Departments.Requests;
using DocumentFlowServer.Api.Features.Departments.Responses;
using DocumentFlowServer.Application.Department;
using DocumentFlowServer.Application.Department.Dtos;

namespace DocumentFlowServer.Api.Features.Departments;

public class DepartmentMappingProfile : Profile
{
    public DepartmentMappingProfile()
    {
        //Get Departments
        CreateMap<GetDepartmentsRequest, DepartmentFilter>();
        CreateMap<PagedDepartmentDto, PagedDepartmentResponse>();
        
        //Create department
        CreateMap<CreateDepartmentRequest, CreateDepartmentDto>();
        
        //Update department
        CreateMap<UpdateDepartmentRequest, UpdateDepartmentDto>();
    }
}