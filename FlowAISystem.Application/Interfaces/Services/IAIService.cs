namespace FlowAISystem.Application.Interfaces.Services;

public interface IAIService
{
    Task<string> AskAsync(string message);
}
