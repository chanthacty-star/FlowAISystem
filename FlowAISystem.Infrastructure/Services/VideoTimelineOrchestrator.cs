using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FlowAISystem.Application.Common.Interfaces;
using FlowAISystem.Application.Common.Models;
using Microsoft.Extensions.Logging;

namespace FlowAISystem.Infrastructure.Services;

public class VideoTimelineOrchestrator : IVideoTimelineOrchestrator
{
    private readonly ITextToSpeechService _ttsService;
    private readonly ISpeechPreparationService _speechPrepService;
    private readonly ILogger<VideoTimelineOrchestrator> _logger;

    public VideoTimelineOrchestrator(
        ITextToSpeechService ttsService,
        ISpeechPreparationService speechPrepService,
        ILogger<VideoTimelineOrchestrator> logger)
    {
        _ttsService = ttsService;
        _speechPrepService = speechPrepService;
        _logger = logger;
    }

    public async Task<VideoTimeline> BuildTimelineAsync(
        string lessonTitle,
        List<(string CodeSnippet, string RawScript)> blocks,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Building video timeline for lesson: '{Title}' with {Count} blocks.", lessonTitle, blocks?.Count ?? 0);

        var segments = new List<TimelineSegment>();
        TimeSpan currentStartTime = TimeSpan.Zero;
        int index = 0;

        if (blocks == null)
        {
            // Fix 1: Provide TimelineId, LessonTitle, Segments, and TotalDuration
            return new VideoTimeline(
                TimelineId: Guid.NewGuid(),
                LessonTitle: lessonTitle,
                Segments: segments,
                TotalDuration: TimeSpan.Zero
            );
        }

        foreach (var (codeSnippet, rawScript) in blocks)
        {
            cancellationToken.ThrowIfCancellationRequested();

            _logger.LogInformation("Processing block {Index} of {Total}", index + 1, blocks.Count);

            // 1. Prepare SSML script
            var prepRequest = new SpeechPreparationRequest(
                RawText: rawScript,
                TargetAudience: "Developer",
                InjectSsmlTags: true,
                VoiceLocale: "en-US"
            );
            var preparedScript = await _speechPrepService.PrepareScriptAsync(prepRequest);

            // 2. Synthesize audio & capture word boundary bookmarks
            var ttsRequest = new TtsRequest(
                Text: preparedScript.SsmlScript,
                VoiceName: "en-US-JennyNeural",
                Style: "cheerful",
                StyleDegree: 1.2
            );
            var ttsResult = await _ttsService.SynthesizeAsync(ttsRequest);

            // 3. Assemble timeline segment
            var segment = new TimelineSegment(
                SequenceOrder: index + 1,
                DisplayCodeSnippet: codeSnippet,
                RawSpeechScript: rawScript,
                PreparedSsml: preparedScript.SsmlScript,
                StartTime: currentStartTime,
                Duration: ttsResult.Duration,
                SynthesizedAudio: ttsResult.AudioData,
                WordTimings: new List<WordBookmark>(ttsResult.WordBookmarks)
            );

            segments.Add(segment);

            currentStartTime += ttsResult.Duration;
            index++;
        }

        _logger.LogInformation("Successfully built timeline. Total Duration: {Duration}", currentStartTime);

        // Fix 2: Provide TimelineId, LessonTitle, Segments, and TotalDuration
        return new VideoTimeline(
            TimelineId: Guid.NewGuid(),
            LessonTitle: lessonTitle,
            Segments: segments,
            TotalDuration: currentStartTime
        );
    }
}