namespace SurveyBasket.Contracts.Results;

public record PollVoteResponse(
    string Title,
    string Sammary ,
    DateOnly StartAt ,
    DateOnly EndAt, 
    IEnumerable<VoteResponse> Votes
 
);
