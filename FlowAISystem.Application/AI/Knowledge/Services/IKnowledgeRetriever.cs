using FlowAISystem.Application.AI.Knowledge.Models;

namespace FlowAISystem.Application.AI.Knowledge.Services;

public interface IKnowledgeRetriever
{
    Task<KnowledgeResult> RetrieveAsync(
        KnowledgeQuery query,
        CancellationToken cancellationToken = default);
}