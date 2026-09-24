using System;

namespace FlowAISystem.Application.Common.Models;

public record WordBookmark(
    string Word,
    TimeSpan Offset,
    TimeSpan Duration,
    uint TextOffset,
    uint WordLength
);