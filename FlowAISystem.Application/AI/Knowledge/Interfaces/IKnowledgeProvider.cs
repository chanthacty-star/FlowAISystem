using FlowAISystem.Application.AI.Knowledge.Models;

namespace FlowAISystem.Application.AI.Knowledge.Interfaces;

public interface IKnowledgeProvider
{
    Task<KnowledgeResult> GetKnowledgeAsync(
        KnowledgeQuery query,
        CancellationToken cancellationToken = default);
}