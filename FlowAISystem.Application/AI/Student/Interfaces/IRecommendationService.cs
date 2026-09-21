using FlowAISystem.Shared.DTOs.AI;
using FlowAISystem.Shared.DTOs.AI.LessonKnowledge;

namespace FlowAISystem.Application.AI.Student.Interfaces;

public interface IRecommendationService
{
    Task<StudentAIResponseDto> GetNextAsync(
        LessonKnowledgeDto currentLesson,
        string? language);
}