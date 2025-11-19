namespace SurveyBasket.Errors;

public static class QuestionErrors
{
    public static readonly Error DuplicatedQuestionContent =
        new Error("Question.DuplicatedContent", "Another question with the same content already exist", StatusCodes.Status409Conflict);



}
