using FlowAISystem.Application.AI.Knowledge.Interfaces;
using FlowAISystem.Application.AI.Knowledge.Models;

namespace FlowAISystem.Application.AI.Knowledge.Services;

public class KnowledgeRetriever : IKnowledgeRetriever
{
    private readonly IEnumerable<IKnowledgeProvider> _providers;

    public KnowledgeRetriever(IEnumerable<IKnowledgeProvider> providers)
    {
        _providers = providers;
    }

    public async Task<KnowledgeResult> RetrieveAsync(
        KnowledgeQuery query,
        CancellationToken cancellationToken = default)
    {
        var results = new List<KnowledgeResult>();

        foreach (var provider in _providers)
        {
            var result = await provider.GetKnowledgeAsync(query, cancellationToken);
            if (!string.IsNullOrWhiteSpace(result.Content))
                results.Add(result);
        }

        return results.OrderByDescending(r => r.RelevanceScore).FirstOrDefault()
            ?? new KnowledgeResult { Source = "KnowledgeRetriever", Content = string.Empty };
    }
}