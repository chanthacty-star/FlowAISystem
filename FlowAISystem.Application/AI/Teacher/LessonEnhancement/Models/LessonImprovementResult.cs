namespace FlowAISystem.Application.AI.Teacher.LessonEnhancement.Models;

public class LessonImprovementResult 
{
    public string ImprovedContent { get; set; } = string.Empty;

    public List<LessonSuggestion> Suggestions { get; set; } = new();

}