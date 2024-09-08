using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Agent.Dal.Data.Entities;

[Table("agent-assignment")]
public class AgentAssignment
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [ForeignKey("Agent")]
    public int AgentId { get; set; }

    [Required]
    public Guid SessionId { get; set; }

    [Required]
    public DateTime AssignedAt { get; set; } = DateTime.UtcNow;

    [Required]
    public bool IsCompleted { get; set; } = false;

    public Agent Agent { get; set; }
}

