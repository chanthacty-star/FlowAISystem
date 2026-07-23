using FlowAISystem.Shared.DTOs.AI;

namespace FlowAISystem.Application.Interfaces.Services;

public interface IStudentAIService
{

    /// <summary>
    /// Process student question
    /// </summary>
    Task<StudentAIResponseDto> AskAsync(
        StudentAIRequestDto request);


}