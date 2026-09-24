//using System.Threading.Tasks;
//using FlowAISystem.Application.Common.Models;

//namespace FlowAISystem.Application.Interfaces;

//public interface ITextToSpeechService
//{
//    Task<SpeechSynthesisResultDto> SynthesizeExpressiveSpeechAsync(
//        string text,
//        string voiceName = "en-US-JennyNeural",
//        string? style = "cheerful",
//        double styleDegree = 1.2);
//}
using System.Threading;
using System.Threading.Tasks;
using FlowAISystem.Application.Common.Models;

namespace FlowAISystem.Application.Common.Interfaces;

public interface ITextToSpeechService
{
    Task<TtsResult> SynthesizeAsync(TtsRequest request, CancellationToken cancellationToken = default);
}