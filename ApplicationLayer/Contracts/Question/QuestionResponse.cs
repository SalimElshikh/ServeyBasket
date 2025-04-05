using ApplicationLayer.Contracts.Answer;

namespace ApplicationLayer.Contracts.Question;

public record QuestionResponse(
    
    int Id ,
    string Content ,
    IEnumerable<AnswerResponse> Answers

    );






