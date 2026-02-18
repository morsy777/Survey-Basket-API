namespace SurveyBasket.Services;

public interface IQuestionService
{
    Task<Result<QuestionResponse>> AddAsync(int pollId, QuestionRequest request, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<QuestionResponse>>> GetAllAsync(int pollId, CancellationToken cancellationToken = default);
    Task<Result<QuestionResponse>> GetAsync(int pollId, int id, CancellationToken cancellationToken = default);
    Task<Result> ToggleStatusAsync(int pollId, int id, CancellationToken cancellationToken = default);
}
