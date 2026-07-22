namespace FlowAISystem.Application.AI.Interfaces;

public interface IGeneralAIHandler
{
    Task<string> HandleAsync(string message);
}