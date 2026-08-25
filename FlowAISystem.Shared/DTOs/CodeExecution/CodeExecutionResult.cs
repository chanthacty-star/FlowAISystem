namespace FlowAISystem.Shared.DTOs.CodeExecution;

public sealed class CodeExecutionResult
{
    public bool Success { get; set; }

    public string Output { get; set; } = string.Empty;

    public string Error { get; set; } = string.Empty;

    public int ExitCode { get; set; }

    public long ExecutionTimeMs { get; set; }

    public Guid ExecutionId { get; set; }

    public CodeExecutionStatus Status { get; set; }
}