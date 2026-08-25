using FlowAISystem.Application.AI.Student.Enums;
using FlowAISystem.Application.AI.Student.Interfaces;
using FlowAISystem.Application.Interfaces.Services;
using FlowAISystem.Shared.DTOs.AI;
using FlowAISystem.Shared.Enums;

namespace FlowAISystem.Application.AI.Student.Handlers;

public class LearnHandler : IStudentAIWorkflowHandler
{
    private readonly ILessonKnowledgeService _lessonService;
    private readonly ILessonRankingService _lessonRankingService;
    private readonly IKeywordExtractor _keywordExtractor;
    private readonly IStudentAIResponseFormatter _formatter;

    public LearnHandler(
        ILessonKnowledgeService lessonService,
        ILessonRankingService lessonRankingService,
        IKeywordExtractor keywordExtractor,
        IStudentAIResponseFormatter formatter)
    {
        _lessonService = lessonService;
        _lessonRankingService = lessonRankingService;
        _keywordExtractor = keywordExtractor;
        _formatter = formatter;
    }

    public StudentIntent Intent
        => StudentIntent.Learn;

    public async Task<StudentAIResponseDto> HandleAsync(
        StudentAIRequestDto request)
    {
        // ==========================================
        // 1. Validate Request
        // ==========================================

        if (request == null ||
            string.IsNullOrWhiteSpace(request.Question))
        {
            return CreateNotFoundResponse(
                request?.Language);
        }

        var question = request.Question.Trim();
        var language = request.Language;

        // ==========================================
        // 2. Extract Keywords
        // ==========================================

        var keywords = _keywordExtractor
            .Extract(question)
            .ToList();

        // ==========================================
        // 3. Validate Keywords
        // ==========================================

        if (!keywords.Any())
        {
            return CreateNotFoundResponse(language);
        }

        // ==========================================
        // 4. Create Search Text
        // ==========================================

        var searchText = string.Join(" ", keywords);

        // ==========================================
        // 5. Search Teacher Lesson Knowledge
        // ==========================================
        
        var lessons = await _lessonService
            .SearchAsync(keywords);

        // ==========================================
        // 6. Rank Lessons
        // ==========================================

        var lesson = _lessonRankingService
            .Rank(question, lessons);

        // ==========================================
        // 7. Return Best Lesson
        // ==========================================

        if (lesson != null)
        {
            return new StudentAIResponseDto
            {
                Answer = _formatter.Format(
                    lesson.Title,
                    lesson.Content,
                    lesson.Difficulty,
                    language),

                Source = "Teacher Lesson Knowledge",

                LessonId = lesson.Id,
                ActivityType = lesson.ActivityType,

                Confidence = 0.90m,

                CreatedAt = DateTime.UtcNow,

            };
        }

        // ==========================================
        // 8. No Matching Lesson
        // ==========================================

        return CreateNotFoundResponse(language);
    }

    // ==========================================
    // Activity Detection
    // ==========================================

    private string? GetActivityType(
        FlowAISystem.Shared.DTOs.AI.LessonKnowledge.LessonKnowledgeDto lesson)
    {
        var title = lesson.Title?.ToLowerInvariant() ?? string.Empty;
        var category = lesson.Category?.ToLowerInvariant() ?? string.Empty;
        var keywords = lesson.Keywords?.ToLowerInvariant() ?? string.Empty;

        // ==========================================
        // Bubble Sort Activity
        // ==========================================

        if (title.Contains("bubble sort") ||
            keywords.Contains("bubble sort"))
        {
            return "BubbleSort";
        }

        return null;
    }

    // ==========================================
    // Not Found Response
    // ==========================================

    private StudentAIResponseDto CreateNotFoundResponse(
        string? language)
    {
        var isKhmer = language == "km-KH";

        return new StudentAIResponseDto
        {
            Answer = isKhmer
                ? "រកមិនឃើញមេរៀនដែលពាក់ព័ន្ធទេ។"
                : "I couldn't find a matching lesson.",

            Source = "Student AI Knowledge Base",

            Confidence = 0.40m,

            CreatedAt = DateTime.UtcNow,

            ActivityType = null
        };
    }
}