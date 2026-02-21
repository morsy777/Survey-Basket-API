namespace SurveyBasket.Errors;

public static class VoteErrors
{
    public static readonly Error DuplicatedVote = 
        new("Vote.DuplicatedVote", "This user already voted before for this poll", StatusCodes.Status409Conflict);
}
