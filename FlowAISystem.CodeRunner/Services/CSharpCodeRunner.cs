using System.Diagnostics;
using System.Text;

using FlowAISystem.Shared.DTOs.CodeExecution;

namespace FlowAISystem.CodeRunner.Services;

public sealed class CSharpCodeRunner
{
    private readonly ILogger<CSharpCodeRunner> _logger;


    // =========================================================
    // Server-owned security limits
    // =========================================================

    private const int MaxCodeLength = 20_000;

    private const int MaxTimeoutSeconds = 5;

    private const int DefaultTimeoutSeconds = 3;

    private const int MaxOutputLength = 100_000;


    // =========================================================
    // Docker configuration
    // =========================================================

    private const string DockerCommand = "docker";

    private const string DockerImage =
        "flowai-csharp-sandbox:latest";

    private const string ContainerWorkingDirectory =
        "/workspace";


    // =========================================================
    // Constructor
    // =========================================================

    public CSharpCodeRunner(
        ILogger<CSharpCodeRunner> logger)
    {
        _logger = logger;
    }


    // =========================================================
    // Execute
    // =========================================================

    public async Task<CodeExecutionResult> ExecuteAsync(
        CodeExecutionRequest request,
        CancellationToken cancellationToken = default)
    {
        var stopwatch =
            Stopwatch.StartNew();

        var executionId =
            Guid.NewGuid();

        string? tempDirectory = null;

        string? containerName = null;


        _logger.LogInformation(
            "Code execution started. ExecutionId={ExecutionId}",
            executionId);


        try
        {
            // =====================================================
            // B3.11 — INVALID REQUEST
            // =====================================================

            if (request == null)
            {
                return Failure(
                    error:
                        "Execution request cannot be null.",

                    stopwatch:
                        stopwatch,

                    executionId:
                        executionId,

                    status:
                        CodeExecutionStatus.InvalidRequest);
            }


            if (string.IsNullOrWhiteSpace(request.Code))
            {
                return Failure(
                    error:
                        "Code cannot be empty.",

                    stopwatch:
                        stopwatch,

                    executionId:
                        executionId,

                    status:
                        CodeExecutionStatus.InvalidRequest);
            }


            // =====================================================
            // B3.11 — CODE TOO LARGE
            // =====================================================

            if (request.Code.Length > MaxCodeLength)
            {
                return Failure(
                    error:
                        $"Code exceeds the maximum allowed size of {MaxCodeLength:N0} characters.",

                    stopwatch:
                        stopwatch,

                    executionId:
                        executionId,

                    status:
                        CodeExecutionStatus.CodeTooLarge);
            }


            // =====================================================
            // Server-owned timeout
            // =====================================================

            var timeoutSeconds =
                Math.Clamp(
                    request.TimeoutSeconds <= 0
                        ? DefaultTimeoutSeconds
                        : request.TimeoutSeconds,
                    1,
                    MaxTimeoutSeconds);


            // =====================================================
            // Temporary workspace
            // =====================================================

            tempDirectory =
                Path.Combine(
                    Path.GetTempPath(),
                    "FlowAISystem-CodeRunner",
                    Guid.NewGuid().ToString("N"));

            Directory.CreateDirectory(
                tempDirectory);


            // =====================================================
            // Project files
            // =====================================================

            var projectFile =
                Path.Combine(
                    tempDirectory,
                    "StudentCode.csproj");

            var sourceFile =
                Path.Combine(
                    tempDirectory,
                    "Program.cs");


            await File.WriteAllTextAsync(
                projectFile,
                CreateProjectFile(),
                cancellationToken);


            await File.WriteAllTextAsync(
                sourceFile,
                request.Code,
                cancellationToken);


            // =====================================================
            // HOST RESTORE
            // =====================================================

            var restoreResult =
                await RunProcessAsync(
                    fileName:
                        "dotnet",

                    arguments:
                    [
                        "restore",
                        projectFile,
                        "--ignore-failed-sources"
                    ],

                    workingDirectory:
                        tempDirectory,

                    timeoutSeconds:
                        timeoutSeconds,

                    cancellationToken:
                        cancellationToken);


            if (restoreResult.Cancelled)
            {
                return Failure(
                    "C# project restore was cancelled.",
                    stopwatch,
                    executionId,
                    CodeExecutionStatus.RestoreCancelled,
                    restoreResult.Output);
            }


            if (restoreResult.TimedOut)
            {
                return Failure(
                    $"C# project restore exceeded the server limit of {timeoutSeconds} second(s).",
                    stopwatch,
                    executionId,
                    CodeExecutionStatus.RestoreTimeout,
                    restoreResult.Output);
            }


            if (restoreResult.OutputLimitExceeded)
            {
                return Failure(
                    $"Restore output exceeded the maximum allowed size of {MaxOutputLength:N0} characters.",
                    stopwatch,
                    executionId,
                    CodeExecutionStatus.OutputLimitExceeded,
                    restoreResult.Output);
            }


            if (restoreResult.ExitCode != 0)
            {
                _logger.LogWarning(
                    "C# restore failed. ExecutionId={ExecutionId}, ExitCode={ExitCode}, Error={Error}",
                    executionId,
                    restoreResult.ExitCode,
                    restoreResult.Error);

                return Failure(
                    SanitizeBuildOrRestoreError(
                        restoreResult.Error,
                        "C# project restore failed."),
                    stopwatch,
                    executionId,
                    CodeExecutionStatus.RestoreFailed,
                    restoreResult.Output);
            }


            // =====================================================
            // HOST BUILD
            // =====================================================

            var buildResult =
                await RunProcessAsync(
                    fileName:
                        "dotnet",

                    arguments:
                    [
                        "build",
                        projectFile,
                        "--no-restore",
                        "--nologo",
                        "-v",
                        "quiet"
                    ],

                    workingDirectory:
                        tempDirectory,

                    timeoutSeconds:
                        timeoutSeconds,

                    cancellationToken:
                        cancellationToken);


            if (buildResult.Cancelled)
            {
                return Failure(
                    "C# compilation was cancelled.",
                    stopwatch,
                    executionId,
                    CodeExecutionStatus.CompilationCancelled,
                    buildResult.Output);
            }


            if (buildResult.TimedOut)
            {
                return Failure(
                    $"C# compilation exceeded the server limit of {timeoutSeconds} second(s).",
                    stopwatch,
                    executionId,
                    CodeExecutionStatus.CompilationTimeout,
                    buildResult.Output);
            }


            if (buildResult.OutputLimitExceeded)
            {
                return Failure(
                    $"Compilation output exceeded the maximum allowed size of {MaxOutputLength:N0} characters.",
                    stopwatch,
                    executionId,
                    CodeExecutionStatus.OutputLimitExceeded,
                    buildResult.Output);
            }


            if (buildResult.ExitCode != 0)
            {
                _logger.LogInformation(
                    "Student compilation failed. ExecutionId={ExecutionId}, ExitCode={ExitCode}",
                    executionId,
                    buildResult.ExitCode);

                return Failure(
                    SanitizeBuildOrRestoreError(
                        buildResult.Error,
                        "The C# code could not be compiled. ្ដសកថហា"),
                    stopwatch,
                    executionId,
                    CodeExecutionStatus.CompilationError,
                    buildResult.Output);
            }


            // =====================================================
            // Verify compiled DLL
            // =====================================================

            var dllPath =
                Path.Combine(
                    tempDirectory,
                    "bin",
                    "Debug",
                    "net10.0",
                    "StudentCode.dll");


            if (!File.Exists(dllPath))
            {
                _logger.LogError(
                    "Compiled student DLL was not found. ExecutionId={ExecutionId}, Path={Path}",
                    executionId,
                    dllPath);

                return Failure(
                    "The compiled student program could not be found.",
                    stopwatch,
                    executionId,
                    CodeExecutionStatus.SandboxError);
            }


            // =====================================================
            // Container name
            // =====================================================

            containerName =
                "flowai-csharp-" +
                executionId.ToString("N");


            // =====================================================
            // B3.5–B3.9 Docker sandbox
            // =====================================================

            var dockerArguments =
                new List<string>
                {
                    "run",

                    "--rm",

                    "--name",
                    containerName,


                    // =================================================
                    // B3.5 — NETWORK ISOLATION
                    // =================================================

                    "--network",
                    "none",


                    // =================================================
                    // B3.6 — FILESYSTEM ISOLATION
                    // =================================================

                    "--read-only",


                    // =================================================
                    // B3.7 — CONTAINER SANDBOX
                    // =================================================

                    "--cap-drop",
                    "ALL",


                    // =================================================
                    // B3.8 — RESOURCE LIMITS
                    // =================================================

                    "--cpus",
                    "0.5",

                    "--memory",
                    "128m",

                    "--memory-swap",
                    "128m",


                    // =================================================
                    // B3.9 — SECURITY HARDENING
                    // =================================================

                    "--security-opt",
                    "no-new-privileges:true",

                    "--pids-limit",
                    "64",


                    // =================================================
                    // Student workspace
                    // =================================================

                    "--mount",
                    $"type=bind,source={tempDirectory},target={ContainerWorkingDirectory}",

                    "--workdir",
                    ContainerWorkingDirectory,


                    // =================================================
                    // Non-root sandbox image
                    // =================================================

                    DockerImage,


                    // =================================================
                    // Execute compiled program
                    // =================================================

                    "dotnet",
                    "bin/Debug/net10.0/StudentCode.dll"
                };


            // =====================================================
            // Execute Docker sandbox
            // =====================================================

            _logger.LogInformation(
                "Starting Docker sandbox. ExecutionId={ExecutionId}, Container={ContainerName}, TimeoutSeconds={TimeoutSeconds}",
                executionId,
                containerName,
                timeoutSeconds);


            var executionResult =
                await RunDockerProcessAsync(
                    arguments:
                        dockerArguments,

                    timeoutSeconds:
                        timeoutSeconds,

                    cancellationToken:
                        cancellationToken,

                    containerName:
                        containerName);


            stopwatch.Stop();


            // =====================================================
            // B3.11 — CANCELLED
            // =====================================================

            if (executionResult.Cancelled)
            {
                _logger.LogWarning(
                    "Code execution cancelled. ExecutionId={ExecutionId}, Container={ContainerName}",
                    executionId,
                    containerName);

                return new CodeExecutionResult
                {
                    Success = false,

                    Output =
                        executionResult.Output,

                    Error =
                        "Code execution was cancelled.",

                    ExitCode =
                        executionResult.ExitCode,

                    ExecutionTimeMs =
                        stopwatch.ElapsedMilliseconds,

                    ExecutionId =
                        executionId,

                    Status =
                        CodeExecutionStatus.Cancelled
                };
            }


            // =====================================================
            // B3.11 — TIMEOUT
            // =====================================================

            if (executionResult.TimedOut)
            {
                _logger.LogWarning(
                    "Code execution timed out. ExecutionId={ExecutionId}, Container={ContainerName}, TimeoutSeconds={TimeoutSeconds}, ExitCode={ExitCode}",
                    executionId,
                    containerName,
                    timeoutSeconds,
                    executionResult.ExitCode);

                return new CodeExecutionResult
                {
                    Success = false,

                    Output =
                        executionResult.Output,

                    Error =
                        $"Code execution exceeded the server limit of {timeoutSeconds} second(s).",

                    ExitCode =
                        executionResult.ExitCode,

                    ExecutionTimeMs =
                        stopwatch.ElapsedMilliseconds,

                    ExecutionId =
                        executionId,

                    Status =
                        CodeExecutionStatus.Timeout
                };
            }


            // =====================================================
            // B3.11 — OUTPUT LIMIT
            // =====================================================

            if (executionResult.OutputLimitExceeded)
            {
                _logger.LogWarning(
                    "Code execution exceeded output limit. ExecutionId={ExecutionId}",
                    executionId);

                return new CodeExecutionResult
                {
                    Success = false,

                    Output =
                        executionResult.Output,

                    Error =
                        $"Program output exceeded the maximum allowed size of {MaxOutputLength:N0} characters.",

                    ExitCode =
                        executionResult.ExitCode,

                    ExecutionTimeMs =
                        stopwatch.ElapsedMilliseconds,

                    ExecutionId =
                        executionId,

                    Status =
                        CodeExecutionStatus.OutputLimitExceeded
                };
            }


            // =====================================================
            // B3.11 — NON-ZERO EXIT CODE
            // =====================================================

            if (executionResult.ExitCode != 0)
            {
                // =================================================
                // Docker infrastructure failure
                // =================================================

                if (IsDockerInfrastructureError(
                    executionResult.Error))
                {
                    _logger.LogError(
                        "Docker infrastructure failure. " +
                        "ExecutionId={ExecutionId}, " +
                        "Container={ContainerName}, " +
                        "ExitCode={ExitCode}, " +
                        "DockerError={DockerError}",
                        executionId,
                        containerName,
                        executionResult.ExitCode,
                        executionResult.Error);

                    return new CodeExecutionResult
                    {
                        Success = false,

                        Output =
                            string.Empty,

                        Error =
                            "Code execution service is temporarily unavailable. Please try again later.",

                        ExitCode =
                            executionResult.ExitCode,

                        ExecutionTimeMs =
                            stopwatch.ElapsedMilliseconds,

                        ExecutionId =
                            executionId,

                        Status =
                            CodeExecutionStatus.InfrastructureError
                    };
                }


                // =================================================
                // Resource-limit classification
                // =================================================

                if (IsResourceLimitError(
                    executionResult.Error,
                    executionResult.ExitCode))
                {
                    _logger.LogWarning(
                        "Code execution hit resource limit. " +
                        "ExecutionId={ExecutionId}, " +
                        "Container={ContainerName}, " +
                        "ExitCode={ExitCode}, " +
                        "Error={Error}",
                        executionId,
                        containerName,
                        executionResult.ExitCode,
                        executionResult.Error);

                    return new CodeExecutionResult
                    {
                        Success = false,

                        Output =
                            executionResult.Output,

                        Error =
                            "Program exceeded the allowed resource limits.",

                        ExitCode =
                            executionResult.ExitCode,

                        ExecutionTimeMs =
                            stopwatch.ElapsedMilliseconds,

                        ExecutionId =
                            executionId,

                        Status =
                            CodeExecutionStatus.ResourceLimitExceeded
                    };
                }


                // =================================================
                // Student runtime failure
                // =================================================

                _logger.LogWarning(
                    "Student program failed. " +
                    "ExecutionId={ExecutionId}, " +
                    "Container={ContainerName}, " +
                    "ExitCode={ExitCode}, " +
                    "RuntimeError={RuntimeError}",
                    executionId,
                    containerName,
                    executionResult.ExitCode,
                    executionResult.Error);


                return new CodeExecutionResult
                {
                    Success = false,

                    Output =
                        executionResult.Output,

                    Error =
                        SanitizeRuntimeError(
                            executionResult.Error),

                    ExitCode =
                        executionResult.ExitCode,

                    ExecutionTimeMs =
                        stopwatch.ElapsedMilliseconds,

                    ExecutionId =
                        executionId,

                    Status =
                        CodeExecutionStatus.RuntimeError
                };
            }


            // =====================================================
            // B3.11 — SUCCESS
            // =====================================================

            _logger.LogInformation(
                "Code execution completed successfully. " +
                "ExecutionId={ExecutionId}, " +
                "Container={ContainerName}, " +
                "ExitCode={ExitCode}, " +
                "ExecutionTimeMs={ExecutionTimeMs}",
                executionId,
                containerName,
                executionResult.ExitCode,
                stopwatch.ElapsedMilliseconds);


            return new CodeExecutionResult
            {
                Success = true,

                Output =
                    executionResult.Output,

                Error =
                    string.Empty,

                ExitCode =
                    executionResult.ExitCode,

                ExecutionTimeMs =
                    stopwatch.ElapsedMilliseconds,

                ExecutionId =
                    executionId,

                Status =
                    CodeExecutionStatus.Success
            };
        }
        catch (OperationCanceledException)
            when (cancellationToken.IsCancellationRequested)
        {
            stopwatch.Stop();


            if (!string.IsNullOrWhiteSpace(containerName))
            {
                TryRemoveDockerContainer(
                    containerName);
            }


            _logger.LogWarning(
                "Code execution cancelled by request. ExecutionId={ExecutionId}",
                executionId);


            return new CodeExecutionResult
            {
                Success = false,

                Output =
                    string.Empty,

                Error =
                    "Code execution was cancelled.",

                ExitCode = -1,

                ExecutionTimeMs =
                    stopwatch.ElapsedMilliseconds,

                ExecutionId =
                    executionId,

                Status =
                    CodeExecutionStatus.Cancelled
            };
        }
        catch (Exception ex)
        {
            stopwatch.Stop();


            // =====================================================
            // IMPORTANT:
            //
            // Log the real exception on the SERVER.
            //
            // Never return ex.Message to the student.
            // =====================================================

            _logger.LogError(
                ex,
                "Unexpected CodeRunner infrastructure error. ExecutionId={ExecutionId}",
                executionId);


            if (!string.IsNullOrWhiteSpace(containerName))
            {
                TryRemoveDockerContainer(
                    containerName);
            }


            return new CodeExecutionResult
            {
                Success = false,

                Output =
                    string.Empty,

                Error =
                    "Code execution service encountered an internal error. Please try again later.",

                ExitCode = -1,

                ExecutionTimeMs =
                    stopwatch.ElapsedMilliseconds,

                ExecutionId =
                    executionId,

                Status =
                    CodeExecutionStatus.InfrastructureError
            };
        }
        finally
        {
            _logger.LogDebug(
                "Cleaning up code execution resources. ExecutionId={ExecutionId}, Container={ContainerName}",
                executionId,
                containerName);


            if (!string.IsNullOrWhiteSpace(containerName))
            {
                TryRemoveDockerContainer(
                    containerName);
            }


            if (!string.IsNullOrWhiteSpace(tempDirectory))
            {
                TryDeleteDirectory(
                    tempDirectory);
            }
        }
    }


    // =========================================================
    // Host process execution
    // =========================================================

    private async Task<ProcessResult> RunProcessAsync(
        string fileName,
        IReadOnlyList<string> arguments,
        string workingDirectory,
        int timeoutSeconds,
        CancellationToken cancellationToken)
    {
        var result =
            new ProcessResult();


        using var process =
            new Process
            {
                StartInfo =
                    new ProcessStartInfo
                    {
                        FileName =
                            fileName,

                        WorkingDirectory =
                            workingDirectory,

                        RedirectStandardOutput =
                            true,

                        RedirectStandardError =
                            true,

                        UseShellExecute =
                            false,

                        CreateNoWindow =
                            true
                    }
            };


        foreach (var argument in arguments)
        {
            process.StartInfo.ArgumentList.Add(
                argument);
        }


        try
        {
            if (!process.Start())
            {
                result.ErrorBuilder.Append(
                    $"Failed to start process: {fileName}");

                return result;
            }
        }
        catch (Exception ex)
        {
            result.ErrorBuilder.Append(
                $"Failed to start process: {ex.Message}");

            return result;
        }


        var stdoutTask =
            ReadOutputAsync(
                process.StandardOutput,
                result,
                isError: false);


        var stderrTask =
            ReadOutputAsync(
                process.StandardError,
                result,
                isError: true);


        using var timeoutCts =
            CancellationTokenSource
                .CreateLinkedTokenSource(
                    cancellationToken);


        timeoutCts.CancelAfter(
            TimeSpan.FromSeconds(
                timeoutSeconds));


        try
        {
            await process.WaitForExitAsync(
                timeoutCts.Token);
        }
        catch (OperationCanceledException)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                result.Cancelled = true;
            }
            else
            {
                result.TimedOut = true;
            }


            TryKillProcessTree(
                process);
        }


        if (!process.HasExited)
        {
            TryKillProcessTree(
                process);
        }


        try
        {
            await Task.WhenAll(
                stdoutTask,
                stderrTask);
        }
        catch
        {
            // Output collection must never break CodeRunner.
        }


        result.ExitCode =
            process.HasExited
                ? process.ExitCode
                : -1;


        return result;
    }


    // =========================================================
    // Docker process execution
    // =========================================================

    private async Task<ProcessResult> RunDockerProcessAsync(
        IReadOnlyList<string> arguments,
        int timeoutSeconds,
        CancellationToken cancellationToken,
        string containerName)
    {
        var result =
            new ProcessResult();


        using var process =
            new Process
            {
                StartInfo =
                    new ProcessStartInfo
                    {
                        FileName =
                            DockerCommand,

                        RedirectStandardOutput =
                            true,

                        RedirectStandardError =
                            true,

                        UseShellExecute =
                            false,

                        CreateNoWindow =
                            true
                    }
            };


        foreach (var argument in arguments)
        {
            process.StartInfo.ArgumentList.Add(
                argument);
        }


        try
        {
            if (!process.Start())
            {
                result.ErrorBuilder.Append(
                    "Failed to start Docker.");

                return result;
            }
        }
        catch (Exception ex)
        {
            result.ErrorBuilder.Append(
                $"Failed to start Docker: {ex.Message}");

            return result;
        }


        var stdoutTask =
            ReadOutputAsync(
                process.StandardOutput,
                result,
                isError: false);


        var stderrTask =
            ReadOutputAsync(
                process.StandardError,
                result,
                isError: true);


        using var timeoutCts =
            CancellationTokenSource
                .CreateLinkedTokenSource(
                    cancellationToken);


        timeoutCts.CancelAfter(
            TimeSpan.FromSeconds(
                timeoutSeconds));


        try
        {
            await process.WaitForExitAsync(
                timeoutCts.Token);
        }
        catch (OperationCanceledException)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                result.Cancelled = true;
            }
            else
            {
                result.TimedOut = true;
            }


            TryRemoveDockerContainer(
                containerName);


            TryKillProcessTree(
                process);
        }


        if (!process.HasExited)
        {
            TryKillProcessTree(
                process);
        }


        try
        {
            await Task.WhenAll(
                stdoutTask,
                stderrTask);
        }
        catch
        {
            // Best effort.
        }


        result.ExitCode =
            process.HasExited
                ? process.ExitCode
                : -1;


        return result;
    }


    // =========================================================
    // Read stdout / stderr
    // =========================================================

    private static async Task ReadOutputAsync(
        StreamReader reader,
        ProcessResult result,
        bool isError)
    {
        var buffer =
            new char[4096];


        while (true)
        {
            int count;


            try
            {
                count =
                    await reader.ReadAsync(
                        buffer,
                        0,
                        buffer.Length);
            }
            catch
            {
                break;
            }


            if (count <= 0)
            {
                break;
            }


            lock (result)
            {
                if (isError)
                {
                    var remainingError =
                        MaxOutputLength -
                        result.ErrorBuilder.Length;


                    if (remainingError <= 0)
                    {
                        result.OutputLimitExceeded =
                            true;

                        continue;
                    }


                    if (count > remainingError)
                    {
                        result.ErrorBuilder.Append(
                            buffer,
                            0,
                            remainingError);

                        result.OutputLimitExceeded =
                            true;
                    }
                    else
                    {
                        result.ErrorBuilder.Append(
                            buffer,
                            0,
                            count);
                    }


                    continue;
                }


                var remaining =
                    MaxOutputLength -
                    result.OutputBuilder.Length;


                if (remaining <= 0)
                {
                    result.OutputLimitExceeded =
                        true;

                    continue;
                }


                if (count > remaining)
                {
                    result.OutputBuilder.Append(
                        buffer,
                        0,
                        remaining);

                    result.OutputLimitExceeded =
                        true;
                }
                else
                {
                    result.OutputBuilder.Append(
                        buffer,
                        0,
                        count);
                }
            }
        }
    }


    // =========================================================
    // B3.11 — Student runtime error sanitization
    // =========================================================

    private static string SanitizeRuntimeError(
        string error)
    {
        if (string.IsNullOrWhiteSpace(error))
        {
            return "Code execution failed.";
        }


        var lines =
            error
                .Replace(
                    "\r\n",
                    "\n",
                    StringComparison.Ordinal)
                .Split(
                    '\n',
                    StringSplitOptions.RemoveEmptyEntries);


        var safeLines =
            new List<string>();


        foreach (var line in lines)
        {
            var trimmed =
                line.Trim();


            if (string.IsNullOrWhiteSpace(trimmed))
            {
                continue;
            }


            // =====================================================
            // Remove stack trace entries
            // =====================================================

            if (trimmed.StartsWith(
                    "at ",
                    StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }


            // =====================================================
            // Remove source location
            // =====================================================

            if (trimmed.StartsWith(
                    "in ",
                    StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }


            // =====================================================
            // Never expose host/container paths
            // =====================================================

            if (ContainsHostPath(trimmed))
            {
                continue;
            }


            // =====================================================
            // Never expose Docker infrastructure information
            // =====================================================

            if (IsDockerInfrastructureError(trimmed))
            {
                continue;
            }


            safeLines.Add(
                trimmed);
        }


        if (safeLines.Count == 0)
        {
            return "Code execution failed.";
        }


        var result =
            string.Join(
                Environment.NewLine,
                safeLines);


        // =====================================================
        // Normalize .NET exception prefix
        // =====================================================

        result =
            result.Replace(
                "Unhandled exception.",
                "Unhandled exception:",
                StringComparison.OrdinalIgnoreCase);


        return result.Trim();
    }


    // =========================================================
    // B3.11 — Build / restore error sanitization
    // =========================================================

    private static string SanitizeBuildOrRestoreError(
        string error,
        string fallback)
    {
        if (string.IsNullOrWhiteSpace(error))
        {
            return fallback;
        }


        var lines =
            error
                .Replace(
                    "\r\n",
                    "\n",
                    StringComparison.Ordinal)
                .Split(
                    '\n',
                    StringSplitOptions.RemoveEmptyEntries);


        var safeLines =
            new List<string>();


        foreach (var line in lines)
        {
            var trimmed =
                line.Trim();


            if (string.IsNullOrWhiteSpace(trimmed))
            {
                continue;
            }


            if (ContainsHostPath(trimmed))
            {
                continue;
            }


            if (IsDockerInfrastructureError(trimmed))
            {
                continue;
            }


            safeLines.Add(
                trimmed);
        }


        if (safeLines.Count == 0)
        {
            return fallback;
        }


        return string.Join(
            Environment.NewLine,
            safeLines);
    }


    // =========================================================
    // B3.11 — Host/container path detection
    // =========================================================

    private static bool ContainsHostPath(
        string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return false;
        }


        // Windows host paths

        if (value.Contains(
                "C:\\Users\\",
                StringComparison.OrdinalIgnoreCase)
            ||
            value.Contains(
                "D:\\Users\\",
                StringComparison.OrdinalIgnoreCase)
            ||
            value.Contains(
                "AppData\\Local\\Temp",
                StringComparison.OrdinalIgnoreCase)
            ||
            value.Contains(
                "FlowAISystem-CodeRunner",
                StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }


        // Linux/container paths

        if (value.Contains(
                "/workspace/",
                StringComparison.OrdinalIgnoreCase)
            ||
            value.Contains(
                "/tmp/",
                StringComparison.OrdinalIgnoreCase)
            ||
            value.Contains(
                "/app/",
                StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }


        return false;
    }


    // =========================================================
    // B3.11 — Docker infrastructure detection
    // =========================================================

    private static bool IsDockerInfrastructureError(
        string error)
    {
        if (string.IsNullOrWhiteSpace(error))
        {
            return false;
        }


        return
            error.Contains(
                "failed to connect to the docker API",
                StringComparison.OrdinalIgnoreCase)

            ||

            error.Contains(
                "dockerDesktopLinuxEngine",
                StringComparison.OrdinalIgnoreCase)

            ||

            error.Contains(
                "Cannot connect to the Docker daemon",
                StringComparison.OrdinalIgnoreCase)

            ||

            error.Contains(
                "Is the docker daemon running",
                StringComparison.OrdinalIgnoreCase)

            ||

            error.Contains(
                "daemon is not running",
                StringComparison.OrdinalIgnoreCase)

            ||

            error.Contains(
                "error during connect",
                StringComparison.OrdinalIgnoreCase)

            ||

            error.Contains(
                "docker daemon",
                StringComparison.OrdinalIgnoreCase)

            ||

            error.Contains(
                "failed to start Docker",
                StringComparison.OrdinalIgnoreCase);
    }


    // =========================================================
    // B3.11 — Resource-limit detection
    // =========================================================

    private static bool IsResourceLimitError(
        string error,
        int exitCode)
    {
        if (exitCode == 137)
        {
            return true;
        }


        if (exitCode == 139)
        {
            return
                error.Contains(
                    "out of memory",
                    StringComparison.OrdinalIgnoreCase)
                ||
                error.Contains(
                    "memory",
                    StringComparison.OrdinalIgnoreCase);
        }


        return
            error.Contains(
                "out of memory",
                StringComparison.OrdinalIgnoreCase)

            ||

            error.Contains(
                "cannot allocate memory",
                StringComparison.OrdinalIgnoreCase)

            ||

            error.Contains(
                "resource temporarily unavailable",
                StringComparison.OrdinalIgnoreCase);
    }


    // =========================================================
    // Remove Docker container
    // =========================================================

    private static void TryRemoveDockerContainer(
        string containerName)
    {
        try
        {
            using var process =
                new Process
                {
                    StartInfo =
                        new ProcessStartInfo
                        {
                            FileName =
                                DockerCommand,

                            UseShellExecute =
                                false,

                            CreateNoWindow =
                                true
                        }
                };


            process.StartInfo.ArgumentList.Add(
                "rm");

            process.StartInfo.ArgumentList.Add(
                "-f");

            process.StartInfo.ArgumentList.Add(
                containerName);


            if (!process.Start())
            {
                return;
            }


            process.WaitForExit(
                milliseconds: 2000);
        }
        catch
        {
            // Best-effort cleanup.
        }
    }


    // =========================================================
    // Kill process tree
    // =========================================================

    private static void TryKillProcessTree(
        Process process)
    {
        try
        {
            if (!process.HasExited)
            {
                process.Kill(
                    entireProcessTree: true);
            }
        }
        catch
        {
            // Best-effort cleanup.
        }
    }


    // =========================================================
    // Project
    // =========================================================

    private static string CreateProjectFile()
    {
        return """
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>

    <OutputType>Exe</OutputType>

    <TargetFramework>net10.0</TargetFramework>

    <ImplicitUsings>enable</ImplicitUsings>

    <Nullable>enable</Nullable>

    <AssemblyName>StudentCode</AssemblyName>

    <RootNamespace>StudentCode</RootNamespace>

    <GenerateAssemblyInfo>false</GenerateAssemblyInfo>

  </PropertyGroup>

</Project>
""";
    }


    // =========================================================
    // Failure helper
    // =========================================================

    private static CodeExecutionResult Failure(
        string error,
        Stopwatch stopwatch,
        Guid executionId,
        CodeExecutionStatus status,
        string output = "")
    {
        stopwatch.Stop();


        return new CodeExecutionResult
        {
            Success = false,

            Output =
                output,

            Error =
                error,

            ExitCode = -1,

            ExecutionTimeMs =
                stopwatch.ElapsedMilliseconds,

            ExecutionId =
                executionId,

            Status =
                status
        };
    }


    // =========================================================
    // Cleanup temporary directory
    // =========================================================

    private static void TryDeleteDirectory(
        string directory)
    {
        try
        {
            if (Directory.Exists(directory))
            {
                Directory.Delete(
                    directory,
                    recursive: true);
            }
        }
        catch
        {
            // Cleanup must never break response.
        }
    }


    // =========================================================
    // Internal process result
    // =========================================================

    private sealed class ProcessResult
    {
        public int ExitCode { get; set; } = -1;

        public bool TimedOut { get; set; }

        public bool Cancelled { get; set; }

        public bool OutputLimitExceeded { get; set; }


        public StringBuilder OutputBuilder { get; } =
            new();


        public StringBuilder ErrorBuilder { get; } =
            new();


        public string Output =>
            OutputBuilder.ToString();


        public string Error =>
            ErrorBuilder.ToString();
    }
}