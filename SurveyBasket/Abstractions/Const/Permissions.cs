namespace SurveyBasket.Abstractions.Const;

public  static class Permissions
{
    public static string Type { get; } = "permissions";

    public const string GetPolls = "polls:read";
    public const string AddPolls = "polls:add";
    public const string UpdatePolls = "polls:update";
    public const string DeletePolls = "polls:delete";
    public const string GetQuestions = "questions:read";
    public const string AddQuestions = "questions:add";
    public const string UpdateQuestions = "questions:update";
    public const string GetUsers = "user:read";
    public const string AddUsers = "user:add";
    public const string UpdateUsers = "user:update";
    public const string GetRoles = "role:read";
    public const string AddRoles = "role:add";
    public const string UpdateRoles = "role:update";

    public const string Results = "result:read";


    public static IList<string> GetAllPermissions() => new List<string>
    {
        GetPolls,
        AddPolls,
        UpdatePolls,
        DeletePolls,
        GetQuestions,
        AddQuestions,
        UpdateQuestions,
        GetUsers,
        AddUsers,
        UpdateUsers,
        GetRoles,
        AddRoles,
        UpdateRoles,
        Results
    };


}
