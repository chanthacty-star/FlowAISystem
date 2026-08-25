namespace FlowAISystem.Shared.DTOs.CodeExecution;

public sealed class CodeExecutionRequest
{
    public string Language { get; set; } = "C#";

    public string Code { get; set; } = string.Empty;

    public int TimeoutSeconds { get; set; } = 3;
}