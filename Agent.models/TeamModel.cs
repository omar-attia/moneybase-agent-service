namespace Agent.Models
{
    public record TeamModel
    {
        public required string Name { get; init; }

        public TimeSpan ShiftStartTime { get; init; }

        public TimeSpan ShiftEndTime { get; init; }

        public bool IsOfficeHours { get; init; }
    }
}
