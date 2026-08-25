namespace FlowAISystem.Application.AI.Student.Quiz.Models;

public class QuizQuestion
{
    public int Number { get; set; }

    public string Question { get; set; }
        = string.Empty;

    public List<string> Options { get; set; }
        = new();

    public string CorrectAnswer { get; set; }
        = string.Empty;

    public string Explanation { get; set; }
        = string.Empty;
}