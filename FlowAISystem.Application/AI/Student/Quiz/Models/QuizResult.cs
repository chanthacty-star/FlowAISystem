namespace FlowAISystem.Application.AI.Student.Quiz.Models;

public class QuizResult
{
    public string Topic { get; set; }
        = string.Empty;

    public string Difficulty { get; set; }
        = "Beginner";

    public List<QuizQuestion> Questions { get; set; }
        = new();
}