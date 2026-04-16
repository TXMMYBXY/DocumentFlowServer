using System.ComponentModel.DataAnnotations;

namespace DocumentFlowServer.Entities.Models.AboutUserModels;

/// <summary>
/// Role model
/// </summary>
public class Role : EntityBase
{
    [Required]
    public string Title { get; set; }
    public string? Description { get; set; }
}