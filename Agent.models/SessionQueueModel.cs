using Agent.Models.Enums;

namespace Agent.Models;

public record SessionQueueModel(Guid SessionId, SessionStatus Status);

