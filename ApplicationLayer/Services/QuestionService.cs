using ApplicationLayer.Abstractions;
using ApplicationLayer.Contracts.Answer;
using ApplicationLayer.Contracts.Question;
using ApplicationLayer.Errors;
using ApplicationLayer.Reposatories;
using DataLayer.Entities;
using DataLayer.Persistence;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ApplicationLayer.Services;

public class QuestionService(ApplicationDbContext context, ICachService cachService, ILogger<QuestionService> logger) : IQuestionService
{
    private readonly ApplicationDbContext _context = context;
    private readonly ICachService _cachService = cachService;
    private readonly ILogger _logger = logger;
    private const string _cachePrefix = "availableQuestions";

    public async Task<Result<IEnumerable<QuestionResponse>>> GetAllAsync(int pollId, CancellationToken cancellationToken)
    {
        var pollIsExist = await _context.Polls.AnyAsync(x => x.Id == pollId, cancellationToken);
        if (!pollIsExist)
            return Result.Failure<IEnumerable<QuestionResponse>>(PollErrors.NotFoundError);

        var question = await _context.Questions
            .Where(x => x.PollId == pollId)
            .Include(x => x.Answers)
            .Select(q => new QuestionResponse(
                q.Id,
                q.Content,
                q.Answers.Select(a => new AnswerResponse(a.Id, a.Content))

            ))
            //.ProjectToType<QuestionResponse>()
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return Result.Success<IEnumerable<QuestionResponse>>(question);

    }
    public async Task<Result<IEnumerable<QuestionResponse>>> GetAvailableAsync(int pollId, string userId, CancellationToken cancellationToken)
    {
        var hasVote = await _context.Votes.AnyAsync(x => x.PollId == pollId && x.UserId == userId, cancellationToken);
        if (hasVote)
            return Result.Failure<IEnumerable<QuestionResponse>>(VoteErrors.DublicatedVote);

        var pollIsExists = await _context.Polls.AnyAsync(x => x.Id == pollId
            && x.IsPublished
            && x.StartAt <= ConvertDateTimeToDateOnly()
            && x.EndAt >= ConvertDateTimeToDateOnly());

        if (!pollIsExists)
            return Result.Failure<IEnumerable<QuestionResponse>>(PollErrors.NotFoundError);
        var cacheKey = $"{_cachePrefix}-{pollId}";

        var cachedQuestion = await _cachService.GetAsync<IEnumerable<QuestionResponse>>(cacheKey, cancellationToken);
        IEnumerable<QuestionResponse> questions = [];

        if (cachedQuestion is null)
        {
            _logger.LogInformation("Select questions from database");
            questions = await _context.Questions
                .Where(x => x.PollId == pollId && x.IsActive)
                .Include(x => x.Answers)
                .Select(q => new QuestionResponse
                (
                    q.Id,
                    q.Content,
                    q.Answers.Where(a => a.IsActive).Select(a => new AnswerResponse(a.Id, a.Content))
                ))
                .AsNoTracking()
                .ToListAsync(cancellationToken);
            await _cachService.SetAsync(cacheKey, questions, cancellationToken);
        }
        else
        {
            _logger.LogInformation("GetQuestionos from cache");
            questions = cachedQuestion;
        }

        return Result.Success(questions);
    }
    public async Task<Result<QuestionResponse>> GetAsync(int pollId, int id, CancellationToken cancellationToken)
    {

        var question = await _context.Questions
            .Where(x => x.PollId == pollId && x.Id == id)
            .Include(x => x.Answers)
            .ProjectToType<QuestionResponse>()
            .AsNoTracking()
            .SingleOrDefaultAsync(cancellationToken);

        if (question is null)
            return Result.Failure<QuestionResponse>(QuestionErrors.NotFoundError);
        return Result.Success<QuestionResponse>(question);

    }
    public async Task<Result<QuestionResponse>> AddAsync(int pollId, QuestionRequest request, CancellationToken cancellationToken)
    {
        var pollIsExist = await _context.Polls.AnyAsync(x => x.Id == pollId, cancellationToken);
        if (!pollIsExist)
            return Result.Failure<QuestionResponse>(PollErrors.NotFoundError);

        var questionIsExists = await _context.Questions.AnyAsync(x => x.Content == request.Content && x.PollId == pollId, cancellationToken);
        if (questionIsExists)
            return Result.Failure<QuestionResponse>(QuestionErrors.DublicatedQuestion);

        var question = request.Adapt<Question>();
        question.PollId = pollId;

        //// request.Answers.ForEach(answer=>question.Answers.Add(new Answer { Content = answer }));
        //question.Answers = request.Answers
        //     .Select(an => new Answer { Content = an})
        //     .ToList();

        await _context.AddAsync(question, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        await _cachService.RemoveAsync($"{_cachePrefix}-{pollId}", cancellationToken);
        return Result.Success(question.Adapt<QuestionResponse>());

    }

    public async Task<Result> UpdateAsync(int pollId, int id, QuestionRequest request, CancellationToken cancellationToken)
    {
        var contentIsExist = await _context.Questions.
            AnyAsync(x => x.PollId == pollId && x.Id != id && x.Content == request.Content);
        if (contentIsExist)
            return Result.Failure(QuestionErrors.DublicatedQuestion);

        var question = await _context.Questions
            .Include(x => x.Answers)
            .SingleOrDefaultAsync(x => x.PollId == pollId && x.Id == id, cancellationToken);
        if (question is null)
            return Result.Failure(QuestionErrors.NotFoundError);

        question.Content = request.Content;


        // Current answer 

        var currentAnswer = question.Answers.Select(x => x.Content).ToList();

        // Add New Answer 
        var newAnswer = request.Answers.Except(currentAnswer).ToList();

        newAnswer.ForEach(a =>
        {
            question.Answers.Add(new Answer { Content = a });
        });



        question.Answers.ToList().ForEach(a =>
        {
            a.IsActive = request.Answers.Contains(a.Content);
        });
        await _context.SaveChangesAsync(cancellationToken);
        await _cachService.RemoveAsync($"{_cachePrefix}-{pollId}", cancellationToken);

        return Result.Success();

    }

    public async Task<Result> ToggleStatusAsync(int pollId, int id, CancellationToken cancellationToken = default)
    {
        var question = await _context.Questions.SingleOrDefaultAsync(x => x.PollId == pollId && x.Id == id, cancellationToken);

        if (question is null)
            return Result.Failure(QuestionErrors.NotFoundError);

        question.IsActive = !question.IsActive;
        await _context.SaveChangesAsync(cancellationToken);
        await _cachService.RemoveAsync($"{_cachePrefix}-{pollId}", cancellationToken);

        return Result.Success();

    }
    public DateOnly ConvertDateTimeToDateOnly()
    {
        return DateOnly.FromDateTime(DateTime.UtcNow);
    }

}
