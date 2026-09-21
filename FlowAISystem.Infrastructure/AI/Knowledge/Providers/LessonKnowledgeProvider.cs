using FlowAISystem.Application.AI.Knowledge.Interfaces;
using FlowAISystem.Application.AI.Knowledge.Models;
using FlowAISystem.Application.Interfaces.Services;
using FlowAISystem.Shared.DTOs.AI.LessonKnowledge;
using FlowAISystem.Shared.Enums;

namespace FlowAISystem.Infrastructure.AI.Knowledge.Providers;

public class LessonKnowledgeProvider : IKnowledgeProvider
{
    private readonly ILessonKnowledgeService _lessonService;

    public LessonKnowledgeProvider(ILessonKnowledgeService lessonService)
    {
        _lessonService = lessonService;
    }

    public async Task<KnowledgeResult> GetKnowledgeAsync(
        KnowledgeQuery query,
        CancellationToken cancellationToken = default)
    {
        // NOTE: ILessonKnowledgeService.SearchAsync has no CancellationToken parameter —
        // the token stops being honored at this boundary. Not a bug, just a known limit.
        var keywords = query.Topic
            .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        var lessons = await _lessonService.SearchAsync(keywords);

        var candidates = lessons.Where(l => l.IsActive).ToList();

        LessonKnowledgeDto? best;
        if (query.Level is not null &&
            Enum.TryParse<LessonDifficulty>(query.Level, ignoreCase: true, out var difficulty))
        {
            // Prefer a lesson matching the requested level; fall back to any active lesson.
            best = candidates.FirstOrDefault(l => l.Difficulty == difficulty)
                   ?? candidates.FirstOrDefault();
        }
        else
        {
            best = candidates.FirstOrDefault();
        }

        if (best is null)
        {
            return new KnowledgeResult
            {
                Content = string.Empty,
                Source = nameof(LessonKnowledgeProvider),
                RelevanceScore = 0
            };
        }

        return new KnowledgeResult
        {
            Content = best.Content,
            Title = best.Title,
            Source = nameof(LessonKnowledgeProvider),
            RelevanceScore = query.Level is not null &&
                              best.Difficulty.ToString().Equals(query.Level, StringComparison.OrdinalIgnoreCase)
                ? 1.0
                : 0.7 // active match, but not a level match
        };
    }
}