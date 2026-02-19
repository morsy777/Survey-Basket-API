
using SurveyBasket.Entities;

namespace SurveyBasket.Services;

public class QuestionService(ApplicationDbContext context) : IQuestionService
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Result<IEnumerable<QuestionResponse>>> GetAllAsync(int pollId, CancellationToken cancellationToken = default)
    {
        var pollIsExist = await _context.Polls.AnyAsync(x => x.Id == pollId);

        if (!pollIsExist)
            return Result.Failure<IEnumerable<QuestionResponse>>(PollErrors.PollNotFound);

        var questions = await _context.Questions
            .Where(x => x.PollId == pollId)
            .Include(x => x.Answers)
            .ProjectToType<QuestionResponse>()
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return Result.Success<IEnumerable<QuestionResponse>>(questions);
    }

    public async Task<Result<QuestionResponse>> GetAsync(int pollId, int id, CancellationToken cancellationToken = default)
    {
        var question = await _context.Questions
            .Where(x => x.Id == id && x.PollId == pollId)
            .Include(x => x.Answers)
            .ProjectToType<QuestionResponse>()
            .AsNoTracking()
            .SingleOrDefaultAsync(cancellationToken);

        if(question is null)
            return Result.Failure<QuestionResponse>(QuestionErrors.QuestionNotFound);

        return Result.Success(question);
    }

    public async Task<Result<QuestionResponse>> AddAsync(int pollId, QuestionRequest request, CancellationToken cancellationToken = default)
    {
        var pollIsExist = await _context.Polls.AnyAsync(x => x.Id == pollId);

        if(!pollIsExist)
            return Result.Failure<QuestionResponse>(PollErrors.PollNotFound);

        var questionIsExist = await _context.Questions.AnyAsync(x => x.Content == request.Content && x.PollId == pollId);

        if (questionIsExist)
            return Result.Failure<QuestionResponse>(QuestionErrors.DuplicatedQuestionContent);

        var question = request.Adapt<Question>();
        question.PollId = pollId;

        // I Replace the next line with mapping
        //request.Answers.ForEach(answer => question.Answers.Add(new Answer { Content = answer }));

        await _context.Questions.AddAsync(question, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success(question.Adapt<QuestionResponse>());
    }

    public async Task<Result> UpdateAsync(int pollId, int id, QuestionRequest request, CancellationToken cancellationToken = default)
    {
        var isContentExist = await _context.Questions
            .AnyAsync(x => x.Content == request.Content && x.PollId == pollId && x.Id != id, cancellationToken);

        if (isContentExist)
            return Result.Failure(QuestionErrors.DuplicatedQuestionContent);

        var question = await _context.Questions
            .Include(x => x.Answers)
            .SingleOrDefaultAsync(x => x.PollId == pollId && x.Id == id, cancellationToken);

        if(question is null)
            return Result.Failure(QuestionErrors.QuestionNotFound);

        _context.Answers.RemoveRange(question.Answers);

        request.Adapt(question);

        question.PollId = pollId;

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();    
    }

    public async Task<Result> ToggleStatusAsync(int pollId, int id, CancellationToken cancellationToken = default)
    {
        var questionIsExist = await _context.Questions
            .AnyAsync(x => x.PollId == pollId && x.Id == id, cancellationToken);

        if (!questionIsExist)
            return Result.Failure(QuestionErrors.QuestionNotFound);

        await _context.Questions
            .Where(x => x.PollId == pollId && x.Id == id)
            .ExecuteUpdateAsync(setters =>
                setters
                    .SetProperty(x => x.IsActive, x => !x.IsActive),
                cancellationToken);

        return Result.Success();
    }

}
