namespace Agent.Models
{
    public record AgentAssignmentModel
    {
        public required Guid SessionId { get; init; }
        public required int AgentId { get; init; }

    }
}
