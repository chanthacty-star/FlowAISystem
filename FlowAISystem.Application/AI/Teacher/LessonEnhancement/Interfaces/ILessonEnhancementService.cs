using FlowAISystem.Application.AI.Teacher.LessonEnhancement.Models;

namespace FlowAISystem.Application.AI.Teacher.LessonEnhancement.Interfaces;

public interface ILessonEnhancementService
{
    Task<LessonAnalysisResult?> AnalyzeAsync(int lessonId);

    Task<LessonImprovementResult?> ImproveAsync(int lessonId);

    Task<string?> GenerateExamplesAsync(int lessonId);

    Task<string?> GeneratePracticeAsync(int lessonId);

    Task<string?> GenerateSummaryAsync(int lessonId);
}