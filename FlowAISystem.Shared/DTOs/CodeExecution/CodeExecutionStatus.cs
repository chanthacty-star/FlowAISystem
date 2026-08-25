namespace FlowAISystem.Shared.DTOs.CodeExecution;

public enum CodeExecutionStatus
{
    // =========================================================
    // Request validation
    // =========================================================

    InvalidRequest = 1,

    CodeTooLarge = 2,


    // =========================================================
    // Restore
    // =========================================================

    RestoreFailed = 10,

    RestoreTimeout = 11,

    RestoreCancelled = 12,


    // =========================================================
    // Compilation
    // =========================================================

    CompilationError = 20,

    CompilationTimeout = 21,

    CompilationCancelled = 22,


    // =========================================================
    // Runtime / sandbox execution
    // =========================================================

    Success = 30,

    RuntimeError = 31,

    Timeout = 32,

    Cancelled = 33,


    // =========================================================
    // Output / resource protection
    // =========================================================

    OutputLimitExceeded = 40,

    ResourceLimitExceeded = 41,


    // =========================================================
    // Docker / sandbox infrastructure
    // =========================================================

    SandboxError = 50,

    DockerError = 51,

    InfrastructureError = 52,


    // =========================================================
    // Unexpected server failure
    // =========================================================

    InternalError = 99
}