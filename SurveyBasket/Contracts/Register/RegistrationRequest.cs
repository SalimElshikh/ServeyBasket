namespace SurveyBasket.Contracts.Register;

public record RegisterRequest(
    string Email,
    string Password,
    string FirstName,
    string LastName,
    string UserName


);
