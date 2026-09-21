using FlowAISystem.Application.AI.Student.Enums;
using FlowAISystem.Application.AI.Student.Interfaces;
using FlowAISystem.Shared.DTOs.AI;

namespace FlowAISystem.Application.AI.Student.Handlers;

public class ThankYouHandler : IStudentAIWorkflowHandler
{
    public StudentIntent Intent
        => StudentIntent.ThankYou;

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
                    ? "\"មិនអីទេ 😊 ខ្ញុំរីករាយដែលអាចជួយអ្នកបាន!\""
                    : "\"You're welcome! 😊 I'm happy to help.\"",

                Source =
                    "Student AI",

                Confidence =
                    1.0m,

                CreatedAt =
                    DateTime.UtcNow
            });
    }
}
