using ApplicationLayer.Abstractions;
using Microsoft.AspNetCore.Http;

namespace ApplicationLayer.Errors;

public class QuestionErrors
{

    public static readonly Error NotFoundError =
        new("Question.NotFound", " There Is Not Found Question With Geven Id ", StatusCodes.Status404NotFound);
    public static readonly Error IsNull =
        new("Question.Null", $" Question Is NuLL " , StatusCodes.Status400BadRequest);

    public static readonly Error DublicatedQuestion =
        new("Question.DublicatedQuestion", $" Another Question wiht the same Content is already exists ", StatusCodes.Status400BadRequest);


}
