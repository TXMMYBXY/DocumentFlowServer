using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DocumentFlowServer.Entities.Models.AboutUserModels;

namespace DocumentFlowServer.Entities.Models;

/// <summary>
/// Model for tracking user authorizations
/// </summary>
public class LoginHistory : EntityBase
{
    [Required]
    public int UserId { get; set; }
    
    [ForeignKey(nameof(UserId))]
    public virtual User User { get; set; }
    
    [Column(TypeName = "datetime")]
    public DateTime? LoginDate { get; set; } = DateTime.UtcNow;
}