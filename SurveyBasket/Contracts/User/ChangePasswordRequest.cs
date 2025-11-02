namespace SurveyBasket.Contracts.User;

public record ChangePasswordRequest(
    string CurrentPassword,
    string NewPassword
);
