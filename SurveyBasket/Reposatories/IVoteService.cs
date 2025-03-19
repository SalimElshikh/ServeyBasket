using SurveyBasket.Contracts.Vote;

namespace SurveyBasket.Reposatories;

public interface IVoteService
{
    Task<Result> AddAsync(int pollId , string userId ,VoteRequest request, CancellationToken cancellationToken);
}
