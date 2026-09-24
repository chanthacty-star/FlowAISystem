//using System;
//using System.Collections.Generic;
//using System.Text.RegularExpressions;
//using System.Threading;
//using System.Threading.Tasks;
//using FlowAISystem.Application.Common.Interfaces;
//using FlowAISystem.Application.Common.Models;
//using Microsoft.JSInterop;

//namespace FlowAISystem.Infrastructure.Services;

//public class LocalTextToSpeechService : ITextToSpeechService
//{
//    private readonly IJSRuntime _jsRuntime;

//    public LocalTextToSpeechService(IJSRuntime jsRuntime)
//    {
//        _jsRuntime = jsRuntime ?? throw new ArgumentNullException(nameof(jsRuntime));
//    }

//    public async Task<TtsResult> SynthesizeAsync(
//        TtsRequest request,
//        CancellationToken cancellationToken = default)
//    {
//        if (request == null)
//        {
//            throw new ArgumentNullException(nameof(request));
//        }

//        // 1. Strip all SSML and XML markup tags to pass raw spoken text to local Web Speech API
//        string plainText = Regex.Replace(request.Text ?? string.Empty, @"<[^>]*>", string.Empty).Trim();

//        if (string.IsNullOrWhiteSpace(plainText))
//        {
//            return new TtsResult(
//                AudioData: Array.Empty<byte>(),
//                WordBookmarks: new List<WordBookmark>()
//            );
//        }

//        // 2. Trigger local Web Speech API synthesis via JSInterop
//        await _jsRuntime.InvokeVoidAsync("localTts.speakText", cancellationToken, plainText, request.VoiceName);

//        // 3. Compute local estimated word boundary timing markers
//        var wordBookmarks = GenerateEstimatedWordBookmarks(plainText);

//        // 4. Return TtsResult (audio plays directly through browser hardware)
//        return new TtsResult(
//            AudioData: Array.Empty<byte>(),
//            WordBookmarks: wordBookmarks
//        );
//    }

//    private static IReadOnlyList<WordBookmark> GenerateEstimatedWordBookmarks(string text)
//    {
//        var bookmarks = new List<WordBookmark>();
//        var words = text.Split(' ', StringSplitOptions.RemoveEmptyEntries);

//        TimeSpan currentOffset = TimeSpan.Zero;
//        TimeSpan averageWordDuration = TimeSpan.FromMilliseconds(320); // ~180 WPM pacing

//        for (uint i = 0; i < words.Length; i++)
//        {
//            string currentWord = words[i];

//            bookmarks.Add(new WordBookmark(
//                Word: currentWord,
//                Offset: currentOffset,
//                Duration: averageWordDuration,
//                TextOffset: i,
//                WordLength: (uint)currentWord.Length
//            ));

//            currentOffset = currentOffset.Add(averageWordDuration);
//        }

//        return bookmarks;
//    }
//}