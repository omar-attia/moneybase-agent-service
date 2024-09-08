namespace Agent.Models.Enums
{
    public enum SeniorityLevel
    {
        Junior = 1,
        MidLevel = 2,
        Senior = 3,
        TeamLead = 4
    }

    public static class SeniorityMultipliers
    {
        public static readonly Dictionary<SeniorityLevel, decimal> Multipliers = new Dictionary<SeniorityLevel, decimal>
        {
            { SeniorityLevel.Junior, 0.4m },
            { SeniorityLevel.MidLevel, 0.6m },
            { SeniorityLevel.Senior, 0.8m },
            { SeniorityLevel.TeamLead, 0.5m }
        };
    }
}
