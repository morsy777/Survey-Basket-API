namespace SurveyBasket.Errors;

public class PollErrors
{
    public static readonly Error PollNotFound =
        new Error("Poll.NotFound", "No poll was found with the given ID", StatusCodes.Status404NotFound);

    public static readonly Error DuplicatedPollTitle =
        new Error("Poll.DuplicatedTitle", "Another poll with the same title already exist", StatusCodes.Status409Conflict);

    //public static readonly Error 
}
