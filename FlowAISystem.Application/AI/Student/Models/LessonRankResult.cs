using FlowAISystem.Shared.DTOs.AI.LessonKnowledge;
namespace FlowAISystem.Application.AI.Student.Models;

public class LessonRankResult
{
    public LessonKnowledgeDto? BestMatch { get; init; }
    public int BestScore { get; init; }
    public int SecondScore { get; init; }
    public bool IsAmbiguous { get; init; }
    public List<LessonKnowledgeDto> TopMatches { get; init; } = new();

    public static LessonRankResult Empty => new();
}