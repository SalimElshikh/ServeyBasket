using ApplicationLayer.Abstractions;
using ApplicationLayer.Contracts.Results;

namespace ApplicationLayer.Reposatories;

public interface IResultService
{
    Task<Result<PollVoteResponse>> GetPollVotesAsync(int pollId , CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<VotesPerDayResponse>>> GetPollVotesPerDayAsync(int pollId, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<VotesPerQuestionResponse>>> GetVotePerQuestionAsync(int pollId , CancellationToken cancellationToken=default);

}
