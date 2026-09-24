using FlowAISystem.Application.Common.Models;

namespace FlowAISystem.Application.Common.Interfaces;

public interface ISpeechPreparationService
{
    Task<PreparedSpeechScript> PrepareScriptAsync(
        SpeechPreparationRequest request,
        CancellationToken cancellationToken = default
    );
}