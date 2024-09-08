using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Agent.Dal.Data.Entities
{
    [Table("pending-queued-session")]
    public class PendingQueuedSession
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public Guid SessionId { get; set; }
    }
}
