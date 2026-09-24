using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FlowAISystem.Application.Common.Models;

namespace FlowAISystem.Application.Common.Interfaces;

public interface IVideoTimelineOrchestrator
{
    Task<VideoTimeline> BuildTimelineAsync(
        string lessonTitle,
        List<(string CodeSnippet, string RawScript)> lessonBlocks,
        CancellationToken cancellationToken = default);
}