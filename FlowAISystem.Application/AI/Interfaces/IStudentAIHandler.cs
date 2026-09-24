namespace FlowAISystem.Application.AI.Interfaces;// for addmin

public interface IStudentAIHandler
{
    Task<string> HandleAsync(string question);
}