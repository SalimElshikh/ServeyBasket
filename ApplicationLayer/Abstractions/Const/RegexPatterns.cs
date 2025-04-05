namespace ApplicationLayer.Abstractions.Const;

public static class RegexPatterns
{
    public const string Password = "^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@._!])[A-Za-z\\d@._!]{8,}$";
}
