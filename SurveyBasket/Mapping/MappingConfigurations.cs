using Microsoft.AspNetCore.Connections.Features;
using Microsoft.AspNetCore.Identity.Data;
using SurveyBasket.Contracts.Question;

namespace SurveyBasket.Mapping;

public class MappingConfigurations : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<QuestionRequest, Question>()
            .Map(dest => dest.Answers, src => src.Answers.Select(answer => new Answer { Content = answer }));


    }
}
