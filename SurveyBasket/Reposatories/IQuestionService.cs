using SurveyBasket.Contracts.Question;

namespace SurveyBasket.Reposatories;

public interface IQuestionService
{
    Task<Result<QuestionResponse>> AddAsync(int pollId , QuestionRequest request , CancellationToken cancellationToken);
    Task<Result<IEnumerable<QuestionResponse>>> GetAllAsync (int pollId , CancellationToken cancellationToken);
    Task<Result<IEnumerable<QuestionResponse>>> GetAvailableAsync (int pollId , string userId , CancellationToken cancellationToken);
    Task<Result<QuestionResponse>> GetAsync (int pollId , int id , CancellationToken cancellationToken);
    Task<Result> ToggleStatusAsync(int pollId, int id, CancellationToken cancellationToken);
    Task<Result> UpdateAsync(int pollId , int id , QuestionRequest request,CancellationToken cancellationToken );
    
}
