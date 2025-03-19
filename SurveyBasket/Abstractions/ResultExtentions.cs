﻿
namespace SurveyBasket.Abstractions;

public static class ResultExtentions
{
    public static ObjectResult ToProblem(this Result result  )
    {
        if (result.IsSuccess)
            throw new InvalidOperationException("Connot Convert Result To Problem");


        var problem = Results.Problem(statusCode: result.Error.statusCode);
        var problemDetails = problem.GetType().GetProperty(nameof(ProblemDetails))!.GetValue(problem) as ProblemDetails;
        problemDetails!.Extensions = new Dictionary<string, object?>
        {
            {
                "errors",
                new[]
                {
                    result.Error.Code,
                    result.Error.Description
                }
            }
        };
        return new ObjectResult(problemDetails);


    }
}
