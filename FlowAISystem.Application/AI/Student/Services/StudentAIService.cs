using FlowAISystem.Application.AI.Student.Handlers;
using FlowAISystem.Application.AI.Student.Interfaces;
using FlowAISystem.Application.AI.Student.Response;
using FlowAISystem.Application.Interfaces.Services;
using FlowAISystem.Shared.DTOs.AI;

namespace FlowAISystem.Application.AI.Student.Services;


public class StudentAIService : IStudentAIService
{

    private readonly IStudentIntentDetector _intentDetector;

    private readonly IEnumerable<IStudentAIWorkflowHandler> _handlers;

    private readonly IStudentAIResponseBuilder _responseBuilder;


    public StudentAIService(
        IStudentIntentDetector intentDetector,
        IEnumerable<IStudentAIWorkflowHandler> handlers,
        IStudentAIResponseBuilder responseBuilder)
    {
        _intentDetector = intentDetector;
        _handlers = handlers;
        _responseBuilder = responseBuilder;
    }

    public async Task<StudentAIResponseDto> AskAsync(
        StudentAIRequestDto request)
    {

        if (string.IsNullOrWhiteSpace(request.Question))
        {
            return _responseBuilder.Create(
                GetText(
                    request.Language,
                    "សូមបញ្ចូលសំណួររបស់អ្នក។",
                    "Please enter your question."),
                "Student AI",
                0.20m);
        }

        var question =
            request.Question.Trim();



        var intent =
            _intentDetector.Detect(question);

        var handler =
            _handlers.FirstOrDefault(
                h => h.Intent == intent);

        if (handler == null) // this mean AI cannot handle the qu, mayby st qu longer, or error piece AI service, then return below will proces to protect
        {
            return _responseBuilder.Create(
                GetText(
                    request.Language,
                    "ខ្ញុំមិនទាន់អាចឆ្លើយសំណួរនេះបានទេណា។",
                    //"I cannot handle this request yet.",
                    $"I’m not sure what you mean by Are you asking about a programming topic, a lesson, or something else?"),
                "Student AI",
                0.50m);
        }

        return await handler.HandleAsync(request); // and if St AI got it, this statement will procesing and connect or rquest to learn handler 
    }
    private string GetText(
        string? language,
        string khmer,
        string english)
    {
        if (!string.IsNullOrWhiteSpace(language)
            && language.StartsWith(
                "km",
                StringComparison.OrdinalIgnoreCase))
        {
            return khmer;
        }

        return english;
    }
}