using System.Text;
using System.Text.RegularExpressions;
using FlowAISystem.Application.Common.Interfaces;
using FlowAISystem.Application.Common.Models;
using Microsoft.Extensions.Logging;

namespace FlowAISystem.Infrastructure.Services;

public class SpeechPreparationServiceV3 : ISpeechPreparationService
{
    private readonly ILogger<SpeechPreparationServiceV3> _logger;

    public SpeechPreparationServiceV3(ILogger<SpeechPreparationServiceV3> logger)
    {
        _logger = logger;
    }

    public async Task<PreparedSpeechScript> PrepareScriptAsync(
        SpeechPreparationRequest request,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Generating V3 SSML-enhanced narration script...");

        await Task.Delay(150, cancellationToken); // Processing latency simulation

        // 1. Transform raw technical code into clean spoken text
        string plainText = TransformCodeToSpokenText(request.RawText);

        // 2. Wrap and augment with SSML elements
        string ssmlText = BuildSsmlMarkup(plainText, request.VoiceLocale);

        int wordCount = plainText.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length;
        TimeSpan duration = TimeSpan.FromMinutes(wordCount / 130.0);

        return new PreparedSpeechScript(
            OriginalText: request.RawText,
            PlainNarrationScript: plainText,
            SsmlScript: ssmlText,
            EstimatedWordCount: wordCount,
            EstimatedSpeakingTime: duration
        );
    }

    private string TransformCodeToSpokenText(string input)
    {
        // Replace variable declarations with natural prose
        string transformed = Regex.Replace(
            input,
            @"int\s+(\w+)\s*=\s*(\d+);",
            "an integer variable called $1, and set it to $2."
        );

        return transformed;
    }

    private string BuildSsmlMarkup(string text, string locale)
    {
        var sb = new StringBuilder();

        sb.AppendLine("<speak version=\"1.0\" xmlns=\"http://www.w3.org/2001/10/synthesis\" xml:lang=\"" + locale + "\">");
        sb.AppendLine("  <voice name=\"en-US-AriaNeural\">");
        sb.AppendLine("    <p>");

        // Replace technical terms with custom substitutions or phonetic aliases
        string markedUp = ApplyTechnicalPronunciations(text);

        // Add explicit pauses after code explanation sentences
        markedUp = Regex.Replace(markedUp, @"\.\s+", ". <break time=\"750ms\"/> ");

        // Add emphasis on key variables
        markedUp = Regex.Replace(markedUp, @"\b(age|variable|integer)\b", "<emphasis level=\"strong\">$1</emphasis>");

        sb.AppendLine($"      {markedUp}");
        sb.AppendLine("    </p>");
        sb.AppendLine("  </voice>");
        sb.AppendLine("</speak>");

        return sb.ToString();
    }

    private string ApplyTechnicalPronunciations(string input)
    {
        // Substitute programming symbols & keywords with exact voice aliases (<sub alias="...">)
        return input
            .Replace("C#", "<sub alias=\"C Sharp\">C#</sub>")
            .Replace(".NET", "<sub alias=\"Dot Net\">.NET</sub>")
            .Replace("tuple", "<sub alias=\"too-pull\">tuple</sub>")
            .Replace("async", "<sub alias=\"ay-sink\">async</sub>");
    }
}