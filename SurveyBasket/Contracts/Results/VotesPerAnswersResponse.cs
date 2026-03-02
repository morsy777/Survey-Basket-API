namespace SurveyBasket.Contracts.Results;

public record VotesPerAnswersResponse(
    string Answer,
    int Count
);
