using ApplicationLayer.Abstractions;
using Microsoft.AspNetCore.Http;

namespace ApplicationLayer.Errors;

public class PollErrors
{

    public static readonly Error NotFoundError =
        new("Poll.NotFound", " There Is Not Found Poll With Geven Id ",StatusCodes.Status404NotFound);
    public static readonly Error IsNull =
        new("Poll.Add", $" Poll Is NuLL " , StatusCodes.Status400BadRequest);
    public static readonly Error AddError =
        new("Poll.Add", $" There Is An Error With in Add Poll In Database " , StatusCodes.Status400BadRequest);
    public static readonly Error DublicatedPoll =
        new("Poll.DublicatedPoll", $" Another poll wiht the same title is already exists ", StatusCodes.Status400BadRequest);


}
