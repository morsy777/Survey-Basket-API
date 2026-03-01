namespace SurveyBasket.Errors;

public static class VoteErrors
{
    public static readonly Error DuplicatedVote = 
        new("Vote.DuplicatedVote", "This user already voted before for this poll", StatusCodes.Status409Conflict);

    public static readonly Error InvalidQuestions =
        new Error("Question.InvalidQuestions", "Invalid Question", StatusCodes.Status400BadRequest);
}
