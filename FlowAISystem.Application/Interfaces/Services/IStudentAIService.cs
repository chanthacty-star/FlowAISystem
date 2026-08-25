using FlowAISystem.Application.AI.Student.Enums;
using FlowAISystem.Application.AI.Student.Interfaces;
using FlowAISystem.Application.AI.Student.Response;
using FlowAISystem.Shared.DTOs.AI;

//namespace FlowAISystem.Application.AI.Student.Services;

namespace FlowAISystem.Application.Interfaces.Services;

public interface IStudentAIService
{

    /// <summary>
    /// Process student question
    /// </summary>
    Task<StudentAIResponseDto> AskAsync(
        StudentAIRequestDto request);


}