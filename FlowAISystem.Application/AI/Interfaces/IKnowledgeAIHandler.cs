namespace FlowAISystem.Application.AI.Interfaces;

public interface IAIKnowledgeHandler
{
    Task<string> HandleAsync(string question);
}