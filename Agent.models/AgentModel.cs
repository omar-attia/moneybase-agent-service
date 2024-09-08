using Agent.Models.Enums;

namespace Agent.Models
{
    public record AgentModel
    {
        public required string Name { get; init; }

        public required string Email { get; init; }

        public required SeniorityLevel Seniority { get; init; }

        public int TeamId { get; init; }

        public bool? IsActive { get; init; } = false;
        
        public bool? IsOverflow { get; init; } = false;
    }
}
