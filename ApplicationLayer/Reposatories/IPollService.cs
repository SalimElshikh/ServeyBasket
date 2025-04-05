using ApplicationLayer.Abstractions;
using ApplicationLayer.Contracts.Polls;
using ApplicationLayer.Contracts.Response;
using System.Threading;

namespace ApplicationLayer.Reposatories;

public interface IPollService
{
    Task<Result<List<PollResponse>>> GetAllAsync( CancellationToken cancellationToken = default);
    Task<Result<List<PollResponse>>> GetCurrentAsync( CancellationToken cancellationToken = default);
    Task<Result<PollResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Result<PollResponse>> AddAsync(PollRequest poll, CancellationToken cancellationToken = default);
    Task<Result> UpdateAsync( int id, PollRequest poll, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync( int id, CancellationToken cancellationToken = default);
    Task<Result> TogglePublishStatusAsync(int id, CancellationToken cancellationToken = default);
    
    
}




