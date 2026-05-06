using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DocumentFlowServer.Entities.Enums;

namespace DocumentFlowServer.Entities.Models.AboutUserModels;

/// <summary>
/// User model
/// </summary>
public class User : EntityBase
{
    [Required]
    public string FullName { get; set; }
    
    [EmailAddress]
    [MaxLength(63)]
    public string Email { get; set; }
    
    [Required]
    public string PasswordHash { get; set; }
    
    public bool IsActive { get; set; } = true;
    
    [Required]
    public int DepartmentId { get; set; }
    
    [ForeignKey("DepartmentId")]
    public virtual Department Department { get; set; }
    
    [Required]
    public Role Role { get; set; }

}