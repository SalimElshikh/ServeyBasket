namespace ApplicationLayer.Contracts.Vote;

public record VoteRequest(
    IEnumerable<VoteAnswerRequest> Answers
);
