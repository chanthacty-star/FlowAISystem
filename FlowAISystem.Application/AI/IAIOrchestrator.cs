namespace FlowAISystem.Application.AI;

public interface IAIOrchestrator
{
    Task<string> HandleRequestAsync(string userMessage, int? userId, CancellationToken ct = default);
}