//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using FlowAISystem.Application.Common.Models;
//using FlowAISystem.Application.Interfaces.Services;
//using Microsoft.CognitiveServices.Speech;
//using Microsoft.Extensions.Configuration;

//namespace FlowAISystem.Infrastructure.Services;

//public class TextToSpeechService : ITextToSpeechService
//{
//    private readonly SpeechConfig _speechConfig;

//    public TextToSpeechService(IConfiguration configuration)
//    {
//        var subscriptionKey = configuration["AzureSpeech:SubscriptionKey"]
//            ?? throw new ArgumentNullException(nameof(configuration), "AzureSpeech:SubscriptionKey is missing in configuration.");
//        var region = configuration["AzureSpeech:Region"]
//            ?? throw new ArgumentNullException(nameof(configuration), "AzureSpeech:Region is missing in configuration.");

//        _speechConfig = SpeechConfig.FromSubscription(subscriptionKey, region);
//    }

//    public async Task<SpeechSynthesisResultDto> SynthesizeExpressiveSpeechAsync(
//        string text,
//        string voiceName = "en-US-JennyNeural",
//        string? style = "cheerful",
//        double styleDegree = 1.2)
//    {
//        var bookmarks = new List<WordBookmark>();

//        using var synthesizer = new SpeechSynthesizer(_speechConfig, null);

//        synthesizer.SynthesisWordBoundary += (sender, e) =>
//        {
//            if (e.BoundaryType == SpeechSynthesisBoundaryType.Word)
//            {
//                bookmarks.Add(new WordBookmark(
//                    Word: e.Text,
//                    Offset: TimeSpan.FromTicks((long)e.AudioOffset),
//                    Duration: TimeSpan.FromMilliseconds(e.Duration.TotalMilliseconds),
//                    TextOffset: e.TextOffset,
//                    WordLength: e.WordLength
//                ));
//            }
//        };

//        string ssml = SsmlBuilder.BuildSsml(
//            text: text,
//            voiceName: voiceName,
//            style: style,
//            styleDegree: styleDegree
//        );

//        using var result = await synthesizer.SpeakSsmlAsync(ssml);

//        if (result.Reason == ResultReason.SynthesizingAudioCompleted)
//        {
//            return new SpeechSynthesisResultDto(
//                IsSuccess: true,
//                AudioData: result.AudioData,
//                Bookmarks: bookmarks
//            );
//        }

//        var cancellation = SpeechSynthesisCancellationDetails.FromResult(result);
//        return new SpeechSynthesisResultDto(
//            IsSuccess: false,
//            AudioData: null,
//            Bookmarks: bookmarks,
//            ErrorMessage: cancellation.ErrorDetails
//        );
//    }
//} //===> becaseu now using AzureTextToSpeechService.cs