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



        if (handler == null)
        {
            return _responseBuilder.Create(
                GetText(
                    request.Language,
                    "ខ្ញុំមិនទាន់អាចឆ្លើយសំណួរនេះបានទេណា។",
                    "I cannot handle this request yet."),
                "Student AI",
                0.50m);
        }



        return await handler.HandleAsync(request);
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