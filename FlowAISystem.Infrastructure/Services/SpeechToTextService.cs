using FlowAISystem.Application.Common.Interfaces;
using FlowAISystem.Application.Common.Models;
using Microsoft.Extensions.Logging;

namespace FlowAISystem.Infrastructure.Services;

public class SpeechToTextService : ISpeechToTextService
{
    private readonly ILogger<SpeechToTextService> _logger;

    public SpeechToTextService(ILogger<SpeechToTextService> logger)
    {
        _logger = logger;
    }

    public async Task<SttResult> TranscribeAsync(SttRequest request, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Transcribing audio stream...");

        // Simulated STT processing (e.g., OpenAI Whisper API or Azure Speech)
        await Task.Delay(400, cancellationToken);

        return new SttResult(
            Text: "This is a transcribed sample response from the audio stream.",
            Confidence: 0.96
        );
    }
}