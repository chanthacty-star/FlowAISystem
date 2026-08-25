using FlowAISystem.Application.AI.Student.Interfaces;

namespace FlowAISystem.Application.AI.Student.Services;

public class StudentAIGenerator : IStudentAIGenerator
{
    public Task<string> GenerateAsync(string prompt)
    {
        // Temporary local/rule-based implementation.
        // Later you can replace this with a local AI provider.

        var response =
            "Student AI generated response:\n\n" +
            prompt;

        return Task.FromResult(response);
    }
}