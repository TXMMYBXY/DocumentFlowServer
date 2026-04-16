using System.ComponentModel.DataAnnotations;

namespace DocumentFlowServer.Entities.Models;

/// <summary>
/// Base class for tables
/// </summary>
public abstract class EntityBase
{
    [Key]
    public int Id { get; set; }
}