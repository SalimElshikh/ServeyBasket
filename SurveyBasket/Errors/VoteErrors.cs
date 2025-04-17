namespace SurveyBasket.Errors;

public class VoteErrors
{

    public static readonly Error NotFoundError =
        new("Vote.NotFound", " There Is Not Found Vote With Geven Id ",StatusCodes.Status404NotFound);
    public static readonly Error IsNull =
        new("Vote.Add", $" Vote Is NuLL " , StatusCodes.Status400BadRequest);
    public static readonly Error InvalidQuestions =
        new("Vote.InvalidQuestions", $" The Questions Is Not Valid " , StatusCodes.Status400BadRequest);
    public static readonly Error DublicatedVote =
        new("Vote.DublicatedVote", $" The User is Already Voted In This Vote  ", StatusCodes.Status400BadRequest);


}
