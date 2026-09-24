using System.IO;

namespace FlowAISystem.Application.Common.Models;

public record SttRequest(
    Stream AudioStream,
    string Language = "en-US"
);

public record SttResult(
    string Text,
    double Confidence
);