namespace FlowAISystem.Application.AI.Student.Interfaces;

public interface IStudentAIGenerator
{
    Task<string> GenerateAsync(string prompt);
}