using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Agent.Dal.Data.Entities;

[Table("agent")]
public class Agent
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public required string Name { get; set; }

    [Required]
    [MaxLength(100)]
    public required string Email { get; set; }
    [Required]
    [ForeignKey("SeniorityMultiplier")]
    public int SeniorityId { get; set; }

    [Required]
    [ForeignKey("Team")]
    public int TeamId { get; set; }

    [Required] public int MaxConcurrentChats { get; set; }

    [Required]
    public bool IsActive { get; set; }

    [Required]
    public bool IsOverflow { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public bool IsDeleted { get; set; } = false;

    public ICollection<AgentAssignment>? Assignments { get; set; }
    public SeniorityMultiplier SeniorityMultiplier { get; set; }
    public Team Team { get; set; }
}