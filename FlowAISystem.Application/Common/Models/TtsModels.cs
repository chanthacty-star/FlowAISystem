using System;
using System.Collections.Generic;

namespace FlowAISystem.Application.Common.Models;

public record TtsRequest(
    string Text,
    string VoiceName = "en-US-JennyNeural",
    string? Style = null,
    double StyleDegree = 1.0
);

public record TtsResult(
    byte[] AudioData,
    string ContentType,
    TimeSpan Duration,
    IReadOnlyList<WordBookmark> WordBookmarks
);