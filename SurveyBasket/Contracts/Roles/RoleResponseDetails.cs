namespace SurveyBasket.Contracts.Roles;

public record RoleResponseDetails(
    string id ,
    string Name , 
    bool IsDeleted ,
    IEnumerable<string> Permissions
    );
