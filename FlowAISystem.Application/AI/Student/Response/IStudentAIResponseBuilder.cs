using FlowAISystem.Shared.DTOs.AI;

namespace FlowAISystem.Application.AI.Student.Response;

public interface IStudentAIResponseBuilder
{
    StudentAIResponseDto Create(
        string answer,
        string source,
        decimal confidence);
}