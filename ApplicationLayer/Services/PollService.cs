using ApplicationLayer.Abstractions;
using ApplicationLayer.Contracts.Polls;
using ApplicationLayer.Contracts.Response;
using ApplicationLayer.Errors;
using ApplicationLayer.Reposatories;
using DataLayer.Entities;
using DataLayer.Persistence;
using Hangfire;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace ApplicationLayer.Services;

public class PollService(ApplicationDbContext context, INotificationService notificationService) : IPollService
{
    private readonly ApplicationDbContext _context = context;
    private readonly INotificationService _notificationService = notificationService;

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

        return currentPollsNow is not null ? Result.Success(currentPollsNow) : Result.Failure<List<PollResponse>>(PollErrors.IsNull);
    }

    public async Task<Result<PollResponse>> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var poll = await _context.Polls.FindAsync(id, cancellationToken);

        return poll is not null ? Result.Success(poll.Adapt<PollResponse>()) : Result.Failure<PollResponse>(PollErrors.NotFoundError);
    }

    public async Task<Result<PollResponse>> AddAsync(PollRequest request, CancellationToken cancellationToken = default)
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

        if (poll.IsPublished && poll.StartAt == DateOnly.FromDateTime(DateTime.UtcNow))
            BackgroundJob.Enqueue(() => _notificationService.SendNewPollsNotifications(poll.Id));


        return Result.Success();

    }


}
