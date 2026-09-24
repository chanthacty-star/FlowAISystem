using System;

namespace FlowAISystem.Application.Common.Models;

public record SpeechPreparationRequest(
    string RawText,
    string TargetAudience = "Beginner Developer",
    bool InjectSsmlTags = true,
    string VoiceLocale = "en-US"
);

public record PreparedSpeechScript(
    string OriginalText,
    string PlainNarrationScript,
    string SsmlScript,
    int EstimatedWordCount,
    TimeSpan EstimatedSpeakingTime
);