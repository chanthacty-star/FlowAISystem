namespace FlowAISystem.Application.AI.Teacher.LessonEnhancement.Models;

public class LessonAnalysisResult
{
    public int Score { get; set; }

    public List<LessonSuggestion> Suggestions { get; set; } = new();

    public List<string> Strengths { get; set; } = new();

    public List<string> MissingAreas { get; set; } = new();
}