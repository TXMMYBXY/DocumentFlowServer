using System.ComponentModel.DataAnnotations;

namespace DocumentFlowServer.Entities.Models
{
    public abstract class EntityBase
    {
        [Key]
        public int Id { get; set; }
    }
}