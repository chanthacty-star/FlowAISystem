namespace FlowAISystem.Application.AI.Interfaces;

public interface IDashboardAIHandler
{
    Task<string> HandleAsync(string question);
}