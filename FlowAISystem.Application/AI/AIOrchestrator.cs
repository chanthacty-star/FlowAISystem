using FlowAISystem.Application.AI.Knowledge.Services;

namespace FlowAISystem.Application.AI;

public class AIOrchestrator : IAIOrchestrator
{
    private readonly IRequestAnalyzer _analyzer;
    private readonly IKnowledgeRetriever _retriever;

    public AIOrchestrator(IRequestAnalyzer analyzer, IKnowledgeRetriever retriever)
    {
        _analyzer = analyzer;
        _retriever = retriever;
    }

    public async Task<string> HandleRequestAsync(string userMessage, int? userId, CancellationToken ct = default)
    {
        var query = _analyzer.Analyze(userMessage, userId);
        var knowledge = await _retriever.RetrieveAsync(query, ct);

        // TEMPORARY: until Piece 10 (IAIGenerator) exists, just hand back what we found.
        return knowledge.Content;
    }
}