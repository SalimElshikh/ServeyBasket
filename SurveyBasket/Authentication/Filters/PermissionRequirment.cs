namespace SurveyBasket.Authentication.Filters;

public class Permissionrequirment(string permission) : IAuthorizationRequirement 
{
    public string Permission { get; } = permission;


}
