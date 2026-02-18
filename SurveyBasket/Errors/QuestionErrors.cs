namespace SurveyBasket.Errors;

public static class QuestionErrors
{
    public static readonly Error DuplicatedQuestionContent =
        new Error("Question.DuplicatedContent", "Another question with the same content already exist", StatusCodes.Status409Conflict);

    public static readonly Error QuestionNotFound =
        new Error("Question.NotFound", "No question was found with the given ID", StatusCodes.Status404NotFound);

}
