namespace FlowAISystem.Application.AI.Teacher.LessonEnhancement.Models;

public class LessonSuggestion
{
    public string Area { get; set; } = string.Empty;

    public string Message { get; set; } = string.Empty;

    public string Priority { get; set; } = string.Empty;

    public override string ToString()
    {
        return $"[{Priority}] {Area}: {Message}";
    }
}