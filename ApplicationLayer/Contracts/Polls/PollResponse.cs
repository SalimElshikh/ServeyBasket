namespace ApplicationLayer.Contracts.Response;

public record PollResponse(
    int Id,
    string Title,
    string Sammary,
    bool IsPublished,
    DateOnly StartAt,
    DateOnly EndAt

);

