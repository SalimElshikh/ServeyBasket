using Microsoft.EntityFrameworkCore.Query;
using ApplicationLayer.Contracts.Vote;
using ApplicationLayer.Errors;
using DataLayer.Persistence;
using ApplicationLayer.Abstractions;
using ApplicationLayer.Reposatories;
using Microsoft.EntityFrameworkCore;
using DataLayer.Entities;
using Mapster;

namespace ApplicationLayer.Services;

public class VoteService(ApplicationDbContext context) : IVoteService
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Result> AddAsync(int pollId, string userId,VoteRequest request, CancellationToken cancellationToken)
    { 
        var hasVote = await _context.Votes.AnyAsync(x => x.PollId == pollId && x.UserId == userId , cancellationToken);
        if (hasVote)
            return Result.Failure(VoteErrors.DublicatedVote);

        var pollIsExist = await _context.Polls.AnyAsync(x=>x.Id == pollId 
            && x.IsPublished 
            && x.StartAt <= DateOnly.FromDateTime(DateTime.UtcNow) 
            && x.EndAt >= DateOnly.FromDateTime(DateTime.UtcNow), cancellationToken);
        if (!pollIsExist)
            return Result.Failure(PollErrors.NotFoundError);

        var questionExists = await _context.Questions
            .Where(x => x.PollId == pollId && x.IsActive)
            .Select(x => x.Id)
            .ToListAsync(cancellationToken);
        if (!request.Answers.Select(x => x.QuestionId).SequenceEqual(questionExists))
            return Result.Failure(VoteErrors.InvalidQuestions);

        var vote = new Vote
        {
            PollId = pollId,
            UserId = userId,
            Answers = request.Answers.Adapt<IEnumerable<VoteAnswer>>().ToList()

        };
        await _context.AddAsync(vote,cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();

    }


}
