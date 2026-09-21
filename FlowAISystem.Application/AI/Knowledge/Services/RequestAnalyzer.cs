using FlowAISystem.Application.AI.Knowledge.Models;

namespace FlowAISystem.Application.AI.Knowledge.Services;

public class RequestAnalyzer : IRequestAnalyzer
{
    // Deliberately dumb level signal — expand as real phrasing shows up in usage.
    private static readonly Dictionary<string, string> LevelKeywords = new(StringComparer.OrdinalIgnoreCase)
    {
        ["beginner"] = "Beginner",
        ["basic"] = "Beginner",
        ["intro"] = "Beginner",
        ["intermediate"] = "Intermediate",
        ["advanced"] = "Advanced",
        ["expert"] = "Advanced"
    };

    public KnowledgeQuery Analyze(string userMessage, int? userId)
    {
        var level = LevelKeywords
            .FirstOrDefault(kv => userMessage.Contains(kv.Key, StringComparison.OrdinalIgnoreCase))
            .Value;

        return new KnowledgeQuery
        {
            Topic = userMessage.Trim(),
            Level = level,
            UserId = userId
        };
    }
}