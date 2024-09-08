namespace Agent.Models;

public record SessionModel(
    Guid Id,
    Guid ActorId,
    string Status,
    DateTime CreatedAt,
    DateTime ExpiryDat,
    DateTime UpdatedAt);

