using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FlowAISystem.Application.Common.Interfaces;
using FlowAISystem.Application.Common.Models;
using FlowAISystem.Infrastructure.Configuration;
using Microsoft.CognitiveServices.Speech;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FlowAISystem.Infrastructure.Services;

public class AzureTextToSpeechService : ITextToSpeechService
{
    private readonly AzureSpeechOptions _options;
    private readonly ILogger<AzureTextToSpeechService> _logger;

    public AzureTextToSpeechService(
        IOptions<AzureSpeechOptions> options,
        ILogger<AzureTextToSpeechService> logger)
    {
        _options = options.Value;
        _logger = logger;
    }

    public async Task<TtsResult> SynthesizeAsync(TtsRequest request, CancellationToken cancellationToken = default)
    {
        var speechConfig = SpeechConfig.FromSubscription(_options.SubscriptionKey, _options.Region);
        speechConfig.SetSpeechSynthesisOutputFormat(SpeechSynthesisOutputFormat.Audio24Khz160KBitRateMonoMp3);

        var wordBookmarks = new List<WordBookmark>();

        using var synthesizer = new SpeechSynthesizer(speechConfig, null);

        // FIX: Event name is WordBoundary (not SynthesisWordBoundary)
        synthesizer.WordBoundary += (s, e) =>
        {
            if (e.BoundaryType == SpeechSynthesisBoundaryType.Word)
            {
                var audioOffset = TimeSpan.FromTicks((long)e.AudioOffset);

                _logger.LogDebug("Word boundary captured: '{Text}' at {Offset}ms", e.Text, audioOffset.TotalMilliseconds);

                wordBookmarks.Add(new WordBookmark(
                    Word: e.Text,
                    Offset: audioOffset,
                    Duration: e.Duration,
                    TextOffset: e.TextOffset,
                    WordLength: e.WordLength
                ));
            }
        };

        // Construct valid SSML with express-as parameters
        string ssml = SsmlBuilder.BuildSsml(
            text: request.Text,
            voiceName: request.VoiceName,
            style: request.Style,
            styleDegree: request.StyleDegree
        );

        _logger.LogInformation("Sending SSML payload to Azure Speech SDK...");

        using var result = await synthesizer.SpeakSsmlAsync(ssml);

        if (result.Reason == ResultReason.SynthesizingAudioCompleted)
        {
            _logger.LogInformation("Azure synthesis completed. Received {Bytes} bytes ({Duration}s)",
                result.AudioData.Length, result.AudioDuration.TotalSeconds);

            return new TtsResult(
                AudioData: result.AudioData,
                ContentType: "audio/mp3",
                Duration: result.AudioDuration,
                WordBookmarks: wordBookmarks
            );
        }

        var cancellation = SpeechSynthesisCancellationDetails.FromResult(result);
        _logger.LogError("Azure Speech synthesis failed: {Error}", cancellation.ErrorDetails);

        throw new InvalidOperationException($"Azure Speech Synthesis Canceled: {cancellation.ErrorDetails}");
    }
}