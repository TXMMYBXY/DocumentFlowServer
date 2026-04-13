using System.ComponentModel.DataAnnotations;

namespace DocumentFlowServer.Entities.Models.AboutUserModels;

public class Role : EntityBase
{
    [Required]
    public string Title { get; set; }
    public string? Description { get; set; }
}