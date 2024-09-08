using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Agent.Dal.Data.Entities
{
    [Table("seniority-multiplier")]
    public class SeniorityMultiplier
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public required string SeniorityLevel { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal MultiplierValue {get; set; }
    }
}
