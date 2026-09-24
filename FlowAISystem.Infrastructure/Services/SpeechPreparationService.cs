//using System.Text.RegularExpressions;
//using FlowAISystem.Application.Common.Interfaces;
//using FlowAISystem.Application.Common.Models;
//using Microsoft.Extensions.Logging;

//namespace FlowAISystem.Infrastructure.Services;

//public class SpeechPreparationService : ISpeechPreparationService
//{
//    private readonly ILogger<SpeechPreparationService> _logger;

//    public SpeechPreparationService(ILogger<SpeechPreparationService> logger)
//    {
//        _logger = logger;
//    }

//    public async Task<PreparedSpeechScript> PrepareScriptAsync(
//        SpeechPreparationRequest request,
//        CancellationToken cancellationToken = default)
//    {
//        _logger.LogInformation("Preparing speech script for text containing code snippets.");

//        // Simulate preparation pipeline / LLM transformation pass
//        await Task.Delay(200, cancellationToken);

//        string narration = TransformCodeToSpokenText(request.RawText);

//        int wordCount = narration.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length;
//        // Average speaking pace: 130 words per minute
//        double speakingMinutes = wordCount / 130.0;

//        return new PreparedSpeechScript(
//            OriginalText: request.RawText,
//            NarrationScript: narration,
//            EstimatedWordCount: wordCount,
//            EstimatedSpeakingTime: TimeSpan.FromMinutes(speakingMinutes)
//        );
//    }

//    private string TransformCodeToSpokenText(string input)
//    {
//        // Rule 1: Replace simple assignment expressions like "int age = 20;"
//        string transformed = Regex.Replace(
//            input,
//            @"int\s+(\w+)\s*=\s*(\d+);",
//            "an integer called $1, and give it the value $2."
//        );

//        // Rule 2: Convert common symbols into natural pauses or descriptions
//        transformed = transformed
//            .Replace("==", " equals ")
//            .Replace("!=", " does not equal ")
//            .Replace(";", "");

//        return transformed;
//    }
//}