namespace FlowAISystem.Application.AI.Interfaces;

public interface IStudentAIHandler
{
    Task<string> HandleAsync(string question);
}