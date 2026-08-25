using FlowAISystem.Shared.DTOs.AI.LessonKnowledge;
using FlowAISystem.Application.AI.Teacher.LessonEnhancement.Models;

namespace FlowAISystem.Application.AI.Teacher.LessonEnhancement.Interfaces;


public interface ILessonEnhancementAgent
{
    Task<LessonAnalysisResult> AnalyzeAsync( LessonKnowledgeDto lesson);

    Task<LessonImprovementResult> ImproveAsync(LessonKnowledgeDto lesson);

    Task<string> GenerateExamplesAsync(LessonKnowledgeDto lesson);

    Task<string> GeneratePracticeAsync( LessonKnowledgeDto lesson);

    Task<string> GenerateSummaryAsync( LessonKnowledgeDto lesson);
}