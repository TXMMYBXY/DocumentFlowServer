using System.ComponentModel.DataAnnotations;
using DocumentFlowServer.Entities.Models.AboutUserModels;

namespace DocumentFlowServer.Entities.Models;

public class Department : EntityBase
{
    [Required]
    public string Title { get; set; }
    public string? Description { get; set; }
    public virtual ICollection<User>? Employees { get; set; }
}