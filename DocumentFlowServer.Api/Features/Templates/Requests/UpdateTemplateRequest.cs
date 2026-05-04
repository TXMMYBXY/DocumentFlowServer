using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace DocumentFlowServer.Api.Features.Templates.Requests;

public class UpdateTemplateRequest
{
    [FromForm(Name="File")]
    public string? Title { get; set; }

    [FromForm(Name="File")]
    public IFormFile? File { get; set; }
}