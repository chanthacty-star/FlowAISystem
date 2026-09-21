using FlowAISystem.Application.AI.Student.Enums;
using FlowAISystem.Application.AI.Student.Interfaces;
using FlowAISystem.Shared.DTOs.AI;

namespace FlowAISystem.Application.AI.Student.Handlers;

public class UnknownHandler : IStudentAIWorkflowHandler
{

    public StudentIntent Intent
        => StudentIntent.Unknown;

    public Task<StudentAIResponseDto> HandleAsync(
        StudentAIRequestDto request)
    {

        var isKhmer =
            request.Language?
                .StartsWith(
                    "km",
                    StringComparison.OrdinalIgnoreCase)
            == true;

        return Task.FromResult(
            new StudentAIResponseDto
            {
                Answer = isKhmer
                    ? "ខ្ញុំមិនទាន់យល់ថាអ្នកចង់សួរអំពីអ្វីទេ។ សូមសរសេរប្រធានបទ ឬសំណួរឱ្យច្បាស់ជាងនេះ។ ឧទាហរណ៍៖ «ពន្យល់អំពី C#» ឬ «តើ C# Variable ជាអ្វី?»"
                    : "I'm not sure what you'd like to ask about yet. Please provide a topic or a more specific question. For example: \"Explain C#\" or \"What is a C# variable?\"",

                Source = "Student AI",

                Confidence = 0.2m,

                CreatedAt = DateTime.UtcNow
            });
    }
}