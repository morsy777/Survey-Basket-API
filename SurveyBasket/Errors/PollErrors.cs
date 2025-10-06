namespace SurveyBasket.Errors;

public class PollErrors
{
    public static readonly Error PollNotFound =
        new Error("Poll.NotFound", "No poll was found with the given ID");
}
