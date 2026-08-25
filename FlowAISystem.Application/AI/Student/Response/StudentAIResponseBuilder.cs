using FlowAISystem.Shared.DTOs.AI;

namespace FlowAISystem.Application.AI.Student.Response;

public class StudentAIResponseBuilder
    : IStudentAIResponseBuilder
{

    public StudentAIResponseDto Create(
        string answer,
        string source,
        decimal confidence)
    {
        return new StudentAIResponseDto
        {
            Answer = answer,
            Source = source,
            Confidence = confidence,
            CreatedAt = DateTime.UtcNow
        };
    }
}