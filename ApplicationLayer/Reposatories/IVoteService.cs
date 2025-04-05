using ApplicationLayer.Abstractions;
using ApplicationLayer.Contracts.Vote;

namespace ApplicationLayer.Reposatories;

public interface IVoteService
{
    Task<Result> AddAsync(int pollId , string userId ,VoteRequest request, CancellationToken cancellationToken);
}
