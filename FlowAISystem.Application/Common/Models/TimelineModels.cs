using System;
using System.Collections.Generic;

namespace FlowAISystem.Application.Common.Models;

public record VideoTimeline(
    Guid TimelineId,
    string LessonTitle,
    List<TimelineSegment> Segments,
    TimeSpan TotalDuration
);

public record TimelineSegment(
    int SequenceOrder,
    string DisplayCodeSnippet,       // The code shown on screen
    string RawSpeechScript,          // Raw text
    string PreparedSsml,             // SSML script with pauses/emphasis
    TimeSpan StartTime,
    TimeSpan Duration,
    byte[]? SynthesizedAudio,        // Audio binary output
    List<WordBookmark>? WordTimings  // Detailed word-level sync metadata
);

//public record WordBookmark(
//    string Word,
//    TimeSpan Offset,
//    TimeSpan Duration
//);