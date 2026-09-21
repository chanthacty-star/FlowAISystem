using FlowAISystem.Application.AI.Student.Enums;
using FlowAISystem.Application.AI.Student.Interfaces;
using FlowAISystem.Shared.DTOs.AI;

namespace FlowAISystem.Application.AI.Student.Handlers;

public class GeneralHandler : IStudentAIWorkflowHandler
{

    public StudentIntent Intent
        => StudentIntent.General;

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
                ? "តើអ្នកចង់ស្វែងយល់អំពីប្រធានបទនេះយ៉ាងដូចម្តេច? អ្នកអាចសួរដូចជា «ពន្យល់អំពី C#» ឬ «C# មានអ្វីខ្លះ?»"
                : "What would you like to learn about this topic? You can ask something like \"Explain C#\" or \"What are C# variables?\"",


                Source =
                    "Student AI",


                Confidence =
                    1.0m,


                CreatedAt =
                    DateTime.UtcNow
            });
    }
}