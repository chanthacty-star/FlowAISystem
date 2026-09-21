using FlowAISystem.Application.AI.Knowledge.Interfaces;
using FlowAISystem.Application.AI.Knowledge.Models;

namespace FlowAISystem.Application.AI.Knowledge.Services;

public class LessonKnowledgeProvider : IKnowledgeProvider
{
    public Task<KnowledgeResult> GetKnowledgeAsync(
        KnowledgeQuery query,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var result = new KnowledgeResult
        {
            Content = $"Lesson knowledge placeholder for: {query.Topic}",
            Source = "LessonKnowledgeProvider",
            Title = "Sample Lesson Knowledge",
            RelevanceScore = 1.0
        };

        return Task.FromResult(result);
    }
}
