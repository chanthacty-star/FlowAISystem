using FlowAISystem.CodeRunner.Services;
using FlowAISystem.Shared.DTOs.CodeExecution;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<CSharpCodeRunner>();

var app = builder.Build();


// =========================================================
// Health
// =========================================================

app.MapGet(
    "/health",
    () =>
        Results.Ok(
            new
            {
                service = "FlowAISystem.CodeRunner",
                status = "healthy"
            }));


// =========================================================
// Execute C#
// =========================================================

app.MapPost(
    "/execute",
    async (
        CodeExecutionRequest request,
        CSharpCodeRunner runner,
        CancellationToken cancellationToken) =>
    {
        if (string.IsNullOrWhiteSpace(request.Code))
        {
            return Results.BadRequest(
                new
                {
                    error = "Code cannot be empty."
                });
        }

        var result =
            await runner.ExecuteAsync(
                request,
                cancellationToken);

        return Results.Ok(result);
    });


app.Run();