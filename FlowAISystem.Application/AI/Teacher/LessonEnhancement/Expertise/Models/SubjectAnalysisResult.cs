namespace FlowAISystem.Application.AI.Teacher.LessonEnhancement.Expertise.Models;

public class SubjectAnalysisResult
{
    public string Subject { get; set; } = string.Empty;

    public int ScoreAdjustment { get; set; }

    public List<string> Strengths { get; set; } = new();

    public List<string> MissingAreas { get; set; } = new();

    public List<string> Suggestions { get; set; } = new();
}