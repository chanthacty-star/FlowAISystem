using FlowAISystem.Application.AI.Student.Enums;
using FlowAISystem.Application.AI.Student.Interfaces;
using FlowAISystem.Application.Interfaces.Services;
using FlowAISystem.Shared.DTOs.AI;

namespace FlowAISystem.Application.AI.Student.Handlers; // Control flow 

public class RecommendationHandler : IStudentAIWorkflowHandler
{
    private readonly ILessonKnowledgeService _lessonService;
    private readonly IRecommendationService _recommendationService;

    public RecommendationHandler(
        ILessonKnowledgeService lessonService,
        IRecommendationService recommendationService)
    {
        _lessonService = lessonService;
        _recommendationService = recommendationService;
    }

    public StudentIntent Intent => StudentIntent.Recommendation;

    public async Task<StudentAIResponseDto> HandleAsync(StudentAIRequestDto request)
    {
        var language = request.Language;

        if (!request.LessonId.HasValue)
        {
            return CreateResponse(
                GetText(language,
                    "សូមរៀនមេរៀនណាមួយសិន ដើម្បីឲ្យខ្ញុំណែនាំមេរៀនបន្ទាប់។",
                    "Please open a lesson first so I can recommend what's next."),
                0.50m);
        }

        var currentLesson = await _lessonService.GetLessonForAIAsync(request.LessonId.Value);

        if (currentLesson == null)
        {
            return CreateResponse(
                GetText(language,
                    "ខ្ញុំរកមិនឃើញមេរៀនបច្ចុប្បន្នទេ។",
                    "I couldn't find your current lesson."),
                0.40m);
        }

        return await _recommendationService.GetNextAsync(currentLesson, language);
    }

    private StudentAIResponseDto CreateResponse(string answer, decimal confidence)
        => new()
        {
            Answer = answer,
            Source = "Recommendation",
            Confidence = confidence,
            CreatedAt = DateTime.UtcNow
        };

    private string GetText(string? language, string khmer, string english)
        => !string.IsNullOrWhiteSpace(language) &&
           language.StartsWith("km", StringComparison.OrdinalIgnoreCase)
            ? khmer
            : english;
}