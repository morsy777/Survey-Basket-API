namespace SurveyBasket.Contracts.User;

public record UserProfileResponse(
    string Email,    
    string Username,    
    string FirstName,    
    string LastName
);