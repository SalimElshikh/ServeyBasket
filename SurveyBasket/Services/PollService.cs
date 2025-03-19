using SurveyBasket.Contracts.Polls;
using SurveyBasket.Entities;
using SurveyBasket.Errors;

namespace SurveyBasket.Services;

public class PollService(ApplicationDbContext context) : IPollService
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Result<List<PollResponse>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var result = await _context.Polls
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var response = result.Select(p => p.Adapt<PollResponse>()).ToList();

        return response.Any()
            ? Result.Success(response)
            : Result.Failure<List<PollResponse>>(PollErrors.NotFoundError);
    }

    public async Task<Result<List<PollResponse>>> GetCurrentAsync(CancellationToken cancellationToken = default)
    {
        var currentPollsNow = await _context.Polls
        .Where(x => x.IsPublished && x.StartAt <= DateOnly.FromDateTime(DateTime.UtcNow) && x.EndAt >= DateOnly.FromDateTime(DateTime.UtcNow))
        .AsNoTracking()
        .ProjectToType<PollResponse>()
        .ToListAsync(cancellationToken);

        return currentPollsNow is not null ? Result.Success(currentPollsNow) : Result.Failure<List<PollResponse>>(PollErrors.IsNull) ;
    }

    public async Task<Result<PollResponse>> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var poll = await _context.Polls.FindAsync(id, cancellationToken);

        return poll is not null ? Result.Success(poll.Adapt<PollResponse>()) : Result.Failure<PollResponse>(PollErrors.NotFoundError);
    }

    public async Task<Result<PollResponse>> AddAsync(PollRequest request , CancellationToken cancellationToken = default)
    {
        var poll = request.Adapt<Poll>();
        try
        {
            await _context.AddAsync(poll, cancellationToken);
            await _context.SaveChangesAsync();
            return Result.Success(poll.Adapt<PollResponse>());
        }
        catch
        {
            return Result.Failure<PollResponse>(PollErrors.AddError);
        }
    }




    public async Task<Result> UpdateAsync(int id, PollRequest poll, CancellationToken cancellationToken)
    {
        var currentPoll = await _context.Polls.FindAsync(id, cancellationToken);

        if (currentPoll is null)
            return Result.Failure(PollErrors.NotFoundError);

        currentPoll.Title = poll.Title;
        currentPoll.Sammary = poll.Sammary;
        currentPoll.StartAt = poll.StartAt;
        currentPoll.EndAt = poll.EndAt;
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }




    public async Task<Result> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var poll = await _context.Polls.FindAsync(id, cancellationToken);

        if (poll is null)
            return Result.Failure(PollErrors.NotFoundError);

        _context.Remove(poll);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result> TogglePublishStatusAsync(int id, CancellationToken cancellationToken = default)
    {
        var poll = await _context.Polls.FindAsync(id, cancellationToken);
        if (poll is null)
            return Result.Failure(PollErrors.NotFoundError);
        poll.IsPublished = !poll.IsPublished;
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();

    }

   
}
