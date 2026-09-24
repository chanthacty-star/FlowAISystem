using FlowAISystem.Application.Common.Models;

namespace FlowAISystem.Application.Common.Interfaces;

public interface ISpeechToTextService
{
    Task<SttResult> TranscribeAsync(SttRequest request, CancellationToken cancellationToken = default);
}