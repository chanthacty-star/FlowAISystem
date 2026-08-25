using FlowAISystem.Application.AI.Student.Enums;
using FlowAISystem.Shared.DTOs.AI;

namespace FlowAISystem.Application.AI.Student.Handlers;


public interface IStudentAIWorkflowHandler
{

    StudentIntent Intent { get; }


    Task<StudentAIResponseDto> HandleAsync(
        StudentAIRequestDto request);

}