using FlowAISystem.Application.AI.Student.Interfaces;
using FlowAISystem.Application.Interfaces.Services;
using FlowAISystem.Shared.DTOs.AI;
using FlowAISystem.Shared.DTOs.AI.LessonKnowledge;

namespace FlowAISystem.Application.AI.Student.Services;

public class RecommendationService : IRecommendationService // that perform bussiness logic
{
    private readonly ILessonKnowledgeService _lessonService;

    public RecommendationService(ILessonKnowledgeService lessonService)
    {
        _lessonService = lessonService;
    }

    public async Task<StudentAIResponseDto> GetNextAsync(
        LessonKnowledgeDto currentLesson,
        string? language)
    {
        if (currentLesson.CourseOfferingId.HasValue)
        {
            var nextLesson = await _lessonService.GetNextLessonAsync(
                currentLesson.CourseOfferingId.Value,
                currentLesson.Order);

            if (nextLesson != null)
            {
                return new StudentAIResponseDto
                {
                    Answer = GetText(language,
                        $"📌 មេរៀនបន្ទាប់៖ \"{nextLesson.Title}\"\n\nចង់ចាប់ផ្តើមទេ?",
                        $"📌 Next up: \"{nextLesson.Title}\"\n\nWant to jump into it?"),
                    Source = "Recommendation",
                    LessonId = nextLesson.Id,
                    Confidence = 0.85m,
                    CreatedAt = DateTime.UtcNow
                };
            }
        }

        var related = await FindRelatedLessonAsync(currentLesson);

        if (related != null)
        {
            return new StudentAIResponseDto
            {
                Answer = GetText(language,
                    $"អ្នកបានបញ្ចប់មេរៀននេះហើយ! សាកល្បងមេរៀនពាក់ព័ន្ធ៖ \"{related.Title}\"",
                    $"You've wrapped this one up! Here's a related topic to try: \"{related.Title}\""),
                Source = "Recommendation",
                LessonId = related.Id,
                Confidence = 0.60m,
                CreatedAt = DateTime.UtcNow
            };
        }

        return new StudentAIResponseDto
        {
            Answer = GetText(language,
                "អបអរសាទរ! អ្នកបានបញ្ចប់មេរៀនទាំងអស់ដែលមានហើយ។",
                "Nice work — you've completed everything available here!"),
            Source = "Recommendation",
            Confidence = 0.70m,
            CreatedAt = DateTime.UtcNow
        };
    }

    private async Task<LessonKnowledgeDto?> FindRelatedLessonAsync(LessonKnowledgeDto currentLesson)
    {
        if (string.IsNullOrWhiteSpace(currentLesson.Category))
        {
            return null;
        }

        var candidates = await _lessonService.SearchAsync(new[] { currentLesson.Category });

        return candidates.FirstOrDefault(x => x.Id != currentLesson.Id);
    }

    private string GetText(string? language, string khmer, string english)
        => !string.IsNullOrWhiteSpace(language) &&
           language.StartsWith("km", StringComparison.OrdinalIgnoreCase)
            ? khmer
            : english;
}