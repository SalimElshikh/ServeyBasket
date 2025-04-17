using SurveyBasket.Errors;
using System.Linq;

namespace SurveyBasket.Services;

public class ResultService(ApplicationDbContext context) : IResultService
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Result<PollVoteResponse>> GetPollVotesAsync(int pollId, CancellationToken cancellationToken = default)
    {
        var pollVotes = await _context.Polls
            .Where(x => x.Id == pollId && x.IsPublished)
            .Select(x => new PollVoteResponse(
                x.Title,
                x.Sammary,
                x.StartAt,
                x.EndAt,
                x.Votes.Select(v => new VoteResponse(
                  $"{v.User.FirstName} {v.User.LastName}",
                  v.SubmittedOn,
                  v.Answers.Select(a => new QuestionAnswerResponse(
                      a.Question.Content,
                      a.Answer.Content

                  ))
                ))

            ))
            .SingleOrDefaultAsync(cancellationToken);
        return pollVotes is null 
            ? Result.Failure<PollVoteResponse>(PollErrors.NotFoundError) 
            : Result.Success(pollVotes);
    }
    public async Task<Result<IEnumerable<VotesPerDayResponse>>> GetPollVotesPerDayAsync(int pollId, CancellationToken cancellationToken = default)
    {
        var pollIsExists = await _context.Polls.AnyAsync(x => x.Id == pollId, cancellationToken);
        if (!pollIsExists)
            return Result.Failure<IEnumerable<VotesPerDayResponse>>(PollErrors.NotFoundError);
        var votesPerDay = await _context.Votes
            .Where(x => x.PollId == pollId)
            .GroupBy(x => new { Date = DateOnly.FromDateTime(x.SubmittedOn) })
            .Select(v => new VotesPerDayResponse(
                v.Key.Date,
                v.Count()
            ))
            .ToListAsync(cancellationToken);


        return Result.Success<IEnumerable<VotesPerDayResponse>>(votesPerDay);
            
        
    }

    public async Task<Result<IEnumerable<VotesPerQuestionResponse>>> GetVotePerQuestionAsync(int pollId, CancellationToken cancellationToken = default)
    {
        var pollIsExists = await _context.Polls.AnyAsync(x => x.Id == pollId, cancellationToken);
        if (!pollIsExists)
            return Result.Failure<IEnumerable<VotesPerQuestionResponse>>(PollErrors.NotFoundError);


        var votePerQuestion = await _context.VoteAnswers
            .Where(x => x.Vote.PollId == pollId)
            .Select(x => new VotesPerQuestionResponse(
                x.Question.Content,
                x.Question.Votes
                    .GroupBy(x => new { AnswersId = x.Answer.Id, AnswerContent = x.Answer.Content })
                    .Select(g => new VotesPerAnswerResponse(
                        g.Key.AnswerContent,
                        g.Count()

                        ))

            ))
            .ToListAsync(cancellationToken);

        return Result.Success<IEnumerable<VotesPerQuestionResponse>>(votePerQuestion);
    }
}
