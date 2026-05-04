using System.Text.Json;
using AutoMapper;
using DocumentFlowServer.Application.Issue.Dtos;
using DocumentFlowServer.Application.Worker.Dtos;
using DocumentFlowServer.Entities.Enums;
using DocumentFlowServer.Entities.Models;

namespace DocumentFlowServer.Application.Common.MappingProfiles;

public class IssueMappingProfile : Profile
{
    public IssueMappingProfile()
    {
        CreateMap<CreateIssueRequestDto, IssueModel>()
            .ForMember(dest => dest.TaskId, opt => opt.MapFrom(_ => Guid.NewGuid()))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(_ => IssueStatus.Pending))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));

        CreateMap<IssueModel, WorkerTaskDto>()
            .ForMember(dest => dest.Data,opt => opt.MapFrom(src => ParseTemplateData(src.TemplateData)));
        
        
    }
    
    private static Dictionary<string, object> ParseTemplateData(string json)
    {
        if (string.IsNullOrWhiteSpace(json))
            return new Dictionary<string, object>();

        var raw = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(json,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

        var result = new Dictionary<string, object>();

        foreach (var (key, value) in raw!)
        {
            result[key] = ExtractValue(value);
        }

        return result;
    }

    private static object? ExtractValue(JsonElement element)
    {
        return element.ValueKind switch
        {
            JsonValueKind.String => element.GetString(),
            JsonValueKind.Number when element.TryGetInt64(out var l) => l,
            JsonValueKind.Number when element.TryGetDouble(out var d) => d,
            JsonValueKind.True => true,
            JsonValueKind.False => false,
            JsonValueKind.Null => null,
            _ => element.ToString()
        };
    }
}